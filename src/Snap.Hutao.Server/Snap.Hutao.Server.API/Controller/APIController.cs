// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.API.Option;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Entity.Unlocker;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Static;
using Swashbuckle.AspNetCore.Annotations;

namespace Snap.Hutao.Server.API.Controller;

[ApiController]
[Route("")]
[ApiExplorerSettings(GroupName = "Misc")]
public class APIController : ControllerBase
{
    private const string QualityHeader = "x-hutao-quality";
    private const string HighQualityValue = "High";

    private static readonly TimeSpan StaticSizeFetchTimeout = TimeSpan.FromSeconds(10);

    private readonly AppOptions appOptions;
    private readonly AppDbContext appDbContext;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<APIController> logger;

    public APIController(AppOptions appOptions, AppDbContext appDbContext, IHttpClientFactory httpClientFactory, ILogger<APIController> logger)
    {
        this.appOptions = appOptions;
        this.appDbContext = appDbContext;
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    [HttpGet("static/raw/{category}/{fileName}")]
    [SwaggerOperation(
        Summary = "Redirects to a raw static image",
        Description = """
            Redirects to the static image host with a 302 response.

            Request fields:
            - category: the image category, for example AvatarIcon.
            - fileName: the image file name, for example UI_AvatarIcon_Ayaka.png.

            Caller notes:
            - Send x-hutao-quality: High to receive the compressed image. Any other value,
              or an omitted header, returns the original image.
            - The response target depends on that header, so cache the images themselves,
              never the redirect.
            """)]
    public IActionResult GetImage(string category, string fileName)
    {
        string baseUrl = IsHighQualityRequested()
            ? appOptions.GetLowRawBaseUrl()
            : appOptions.GetRawBaseUrl();

        return RedirectStatic(baseUrl, category, fileName);
    }

    [HttpGet("static/zip/{fileName}")]
    [SwaggerOperation(
        Summary = "Redirects to a static resource archive",
        Description = """
            Redirects to the static archive host with a 302 response.

            Request fields:
            - fileName: the archive name ending in .zip, for example ItemIcon.zip.

            Caller notes:
            - Send x-hutao-quality: High to receive the compressed archive. Any other value,
              or an omitted header, returns the original archive.
            - Client versions that predate the quality option keep receiving the original
              archive because they do not send the header.
            """)]
    public IActionResult GetZip(string fileName)
    {
        string baseUrl = IsHighQualityRequested()
            ? appOptions.GetLowZipBaseUrl()
            : appOptions.GetZipBaseUrl();

        return RedirectStatic(baseUrl, fileName);
    }

    [HttpGet("static/size")]
    [SwaggerOperation(
        Summary = "Returns the download size of the static resource archives",
        Description = """
            Returns the size manifest published next to the static files.

            Response data fields:
            - original_full: original quality with the full archive set.
            - original_minimum: original quality with the reduced archive set.
            - tiny_full: compressed quality with the full archive set.
            - tiny_minimum: compressed quality with the reduced archive set.

            Caller notes:
            - All sizes are in bytes.
            - A manifest that has not been published yet yields zeros, so callers must
              tolerate a 0 B result.
            """)]
    public async Task<IActionResult> GetSize(CancellationToken token)
    {
        string sizeUrl = appOptions.GetStaticSizeUrl();

        using CancellationTokenSource timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(token);
        timeoutCts.CancelAfter(StaticSizeFetchTimeout);

        try
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient
                .GetAsync(sizeUrl, HttpCompletionOption.ResponseHeadersRead, timeoutCts.Token)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                StaticResourceSizeInformation? information = await response.Content
                    .ReadFromJsonAsync<StaticResourceSizeInformation>(timeoutCts.Token)
                    .ConfigureAwait(false);

                if (information is not null)
                {
                    return Response<StaticResourceSizeInformation>.Success("OK", information);
                }

                logger.LogWarning("The static resource size manifest {Url} contains no usable data.", sizeUrl);
            }
            else
            {
                logger.LogWarning("Fetching the static resource size manifest {Url} returned {StatusCode}.", sizeUrl, (int)response.StatusCode);
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
        {
            logger.LogWarning(ex, "Fetching the static resource size manifest {Url} failed.", sizeUrl);
        }

        // Keep the endpoint usable while the manifest is not published yet.
        return Response<StaticResourceSizeInformation>.Success("OK", new StaticResourceSizeInformation());
    }

    [HttpGet("mgnt/am-i-banned")]
    public IActionResult CheckIfBanned()
    {
        if (HttpContext.Request.Headers.TryGetValue("x-hutao-island-identifier", out var base64UID))
        {
            byte[] uid = Convert.FromBase64String(base64UID.ToString());
            string strUid = Encoding.Unicode.GetString(uid);
            List<UnlockerBanned> unlockerBanneds = appDbContext.UnlockerBanned.Where(x => x.Uid == strUid).ToList();
            if (unlockerBanneds.Count >= 1)
            {
                return Response<object>.Fail(ReturnCode.BannedUid, unlockerBanneds.First().Reason);
            }

            return Response<object>.Success("OK");
        }

        return Response<object>.Fail(ReturnCode.InvalidUploadData, "Invaild UID");
    }

    [HttpGet("ip")]
    public async Task<IActionResult> GetIpInformation()
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        ipAddress ??= "Unknown";

        string division = "Unknown";
        if (!IsLocalIp(ipAddress))
        {
            try
            {
                using HttpClient httpClient = httpClientFactory.CreateClient();
                string url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,regionName,city";
                HttpResponseMessage response = await httpClient.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("status", out JsonElement status) && status.GetString() == "success")
                    {
                        if (root.TryGetProperty("city", out JsonElement city))
                        {
                            division = city.GetString() ?? "Unknown";
                        }
                    }
                }
            }
            catch
            {
            }
        }

        var ipInfo = new IPInformation
        {
            Ip = ipAddress,
            Division = division,
        };

        return Response<IPInformation>.Success("OK", ipInfo);
    }

    [HttpGet("ips")]
    public string GetIpString()
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        return ipAddress ?? "Unknown";
    }

    private static bool IsHighQuality(string value)
    {
        return string.Equals(value, HighQualityValue, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsLocalIp(string ip)
    {
        if (ip == "::1" || ip == "127.0.0.1")
        {
            return true;
        }

        if (ip.StartsWith("192.168.") || ip.StartsWith("10.") || ip.StartsWith("172.16."))
        {
            return true;
        }

        return false;
    }

    private bool IsHighQualityRequested()
    {
        if (Request.Headers.TryGetValue(QualityHeader, out var header) && IsHighQuality(header.ToString()))
        {
            return true;
        }

        // Query fallback keeps the choice testable without a custom header.
        return IsHighQuality(Request.Query["quality"].ToString());
    }

    private IActionResult RedirectStatic(string baseUrl, params string[] segments)
    {
        // The redirect target depends on a request header, so shared caches must not reuse
        // one variant for every caller.
        Response.Headers["Vary"] = QualityHeader;
        return Redirect($"{baseUrl}/{string.Join('/', segments)}");
    }
}
