// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Entity.Unlocker;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Static;

namespace Snap.Hutao.Server.API.Controller;

[ApiController]
[Route("")]
[ApiExplorerSettings(GroupName = "Misc")]
public class APIController : ControllerBase, IDisposable
{
    private readonly AppDbContext appDbContext;
    private readonly HttpClient httpClient;

    public APIController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
        httpClient = new HttpClient();
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }

    [HttpGet("git-repository/all")]
    public async Task<IActionResult> GetAllGitRepositories([FromQuery] string? name = null)
    {
        IQueryable<GitRepository> query = appDbContext.GitRepositories;

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(r => r.Name.Contains(name));
        }

        var gitRepositories = await query
            .ToListAsync()
            .ConfigureAwait(false);

        return Response<List<GitRepository>>.Success("OK", gitRepositories);
    }

    [HttpGet("static/raw/{category}/{fileName}")]
    public IActionResult GetImage(string category, string fileName)
    {
        string baseUrl = "https://static.snaphutaorp.org/static/raw";
        return Redirect($"{baseUrl}/{category}/{fileName}");
    }

    [HttpGet("static/size")]
    public IActionResult GetSize()
    {
        return Response<StaticResourceSizeInformation>.Success("OK", new StaticResourceSizeInformation()
        {
        });
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

    private bool IsLocalIp(string ip)
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
}