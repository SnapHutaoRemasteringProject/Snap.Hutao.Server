// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.API.Option;
using Snap.Hutao.Server.API.Service.Yae;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Snap.Hutao.Server.API.Controller;

[Route("client")]
[ApiController]
public class ClientController : ControllerBase
{
    private const string AchievementFieldIdPrefix = "AchievementFieldId_";
    private const string FeatureCachePrefix = "Client.Feature";

    private readonly AppOptions appOptions;
    private readonly IMemoryCache memoryCache;
    private readonly HttpClient httpClient;
    private readonly YaeMetadataService yaeMetadataService;

    public ClientController(AppOptions appOptions, IMemoryCache memoryCache, HttpClient httpClient, YaeMetadataService yaeMetadataService)
    {
        this.appOptions = appOptions;
        this.memoryCache = memoryCache;
        this.httpClient = httpClient;
        this.yaeMetadataService = yaeMetadataService;
    }

    [HttpGet("{fileName}.json")]
    [SwaggerOperation(
        Summary = "Serve a client feature file under /client, e.g. AchievementFieldId_{gameVersion}.json",
        Description = "Returns the requested file from the static CDN via a 302 redirect when it already exists. " +
            "When the file is missing and the name starts with AchievementFieldId_, it is generated from the upstream " +
            "Yae metadata and cached in memory. The generated response is served with an ETag derived from the game " +
            "version so conditional requests are honored.")]
    public async Task<IActionResult> GetClientFileAsync(string fileName, CancellationToken token)
    {
        string staticUrl = $"{appOptions.StaticClientBaseUrl}{fileName}.json";

        using HttpRequestMessage request = new(HttpMethod.Get, staticUrl);
        HttpResponseMessage? response;
        try
        {
            response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return StatusCode(StatusCodes.Status502BadGateway);
        }

        using (response)
        {
            if (response.IsSuccessStatusCode)
            {
                return Redirect(staticUrl);
            }

            if (response.StatusCode == HttpStatusCode.NotFound && fileName.StartsWith(AchievementFieldIdPrefix, StringComparison.Ordinal))
            {
                return await GenerateAchievementFieldIdAsync(fileName, token).ConfigureAwait(false);
            }

            return StatusCode(StatusCodes.Status502BadGateway);
        }
    }

    private async Task<IActionResult> GenerateAchievementFieldIdAsync(string fileName, CancellationToken token)
    {
        string version = fileName[AchievementFieldIdPrefix.Length..];

        string cacheKey = $"{FeatureCachePrefix}.{fileName}";
        if (memoryCache.TryGetValue(cacheKey, out string? cachedJson))
        {
            return ReturnJson(cachedJson!, version);
        }

        YaeMetadata metadata = await yaeMetadataService.GetMetadataAsync(token).ConfigureAwait(false);
        if (!string.Equals(metadata.Version, version, StringComparison.Ordinal))
        {
            return NotFound();
        }

        string json = yaeMetadataService.BuildAchievementFieldIdJson(metadata);
        memoryCache.Set(cacheKey, json, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        });

        return ReturnJson(json, version);
    }

    private IActionResult ReturnJson(string json, string version)
    {
        string etag = $"\"{version}\"";
        Response.Headers.ETag = etag;
        Response.Headers.CacheControl = "public, max-age=3600";

        if (Request.Headers.IfNoneMatch.Any(value => string.Equals(value, etag, StringComparison.Ordinal)))
        {
            Response.StatusCode = StatusCodes.Status304NotModified;
            return new EmptyResult();
        }

        return Content(json, "application/json");
    }
}
