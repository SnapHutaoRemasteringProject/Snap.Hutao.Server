// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Controller;

[Route("[controller]")]
[ApiController]
public class EnkaController : ControllerBase
{
    private const string EnkaApiBaseUrl = "https://enka.network/api/uid/";
    private const string UserAgent = "Snap Hutao Server/1.0";

    private readonly HttpClient httpClient;

    public EnkaController(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpGet("{uid}")]
    public Task<IActionResult> GetDataAsync(string uid, CancellationToken token)
    {
        return ForwardAsync($"{EnkaApiBaseUrl}{uid}", token);
    }

    [HttpGet("{uid}/info")]
    public Task<IActionResult> GetPlayerInfoAsync(string uid, CancellationToken token)
    {
        return ForwardAsync($"{EnkaApiBaseUrl}{uid}?info", token);
    }

    private async Task<IActionResult> ForwardAsync(string url, CancellationToken token)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd(UserAgent);

        using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);

        Response.StatusCode = (int)response.StatusCode;
        Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
        await response.Content.CopyToAsync(Response.Body, token).ConfigureAwait(false);

        return new EmptyResult();
    }
}
