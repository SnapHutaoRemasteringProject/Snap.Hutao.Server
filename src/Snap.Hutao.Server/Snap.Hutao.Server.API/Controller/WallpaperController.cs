// Copyright (c) Snap.HutaoRemasteringProject. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.HoyoPlay;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Wallpaper;

namespace Snap.Hutao.Server.API.Controller;

[Route("[controller]")]
[ApiController]
public class WallpaperController : ControllerBase
{
    private const string HoyoPlayAllGameBasicInfoUrl = "https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getAllGameBasicInfo?launcher_id=jGHBHlcOq1&language=zh-cn&game_id=1Z8W5NHUQb";
    private const string HoyoPlayUserAgent = "HYPContainer/1.1.4.133";

    private readonly HttpClient httpClient;
    private readonly AppDbContext dbContext;
    private readonly string upstreamUrl = "https://api.gentle.house/wallpaper/";

    public WallpaperController(HttpClient httpClient, AppDbContext dbContext)
    {
        this.httpClient = httpClient;
        this.dbContext = dbContext;
    }

    [HttpGet("hoyoplay")]
    public async Task<IActionResult> GetHoyoplay()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, HoyoPlayAllGameBasicInfoUrl);
        request.Headers.UserAgent.ParseAdd(HoyoPlayUserAgent);

        using HttpResponseMessage message = await httpClient.SendAsync(request);
        message.EnsureSuccessStatusCode();

        OfficialLauncherBackground? background = await message.Content.ReadFromJsonAsync<OfficialLauncherBackground>();
        string[] urls = [.. background?.Data?.GameInfoList?
            .FirstOrDefault(gameInfo => gameInfo.Game?.Biz is "hk4e_cn")?
            .Backgrounds?
            .Select(item => item.Background?.Url)
            .Where(url => !string.IsNullOrEmpty(url))
            .Select(url => url!) ?? []];

        if (urls.Length is 0)
        {
            throw new InvalidOperationException("Failed to fetch wallpaper data.");
        }

        string url = Random.Shared.GetItems(urls, 1)[0];

        Wallpaper wallpaper = new()
        {
            Url = url,
            SourceUrl = "https://hoyoplay.hoyoverse.com/",
            Author = "miHoYo",
            Uploader = "miHoYo",
        };

        if (!dbContext.Wallpapers.Any(w => w.Url == wallpaper.Url))
        {
            dbContext.Wallpapers.Add(new()
            {
                Url = wallpaper.Url,
                SourceUrl = wallpaper.SourceUrl,
                Author = wallpaper.Author,
                Uploader = wallpaper.Uploader,
                Type = "Hoyoplay",
            });

            dbContext.SaveChanges();
        }

        return Response<Wallpaper>.Success("OK", wallpaper);
    }

    [HttpGet("bing")]
    public async Task<IActionResult> GetBing()
    {
        string url = $"{upstreamUrl}bing";
        Response<Wallpaper> response = await httpClient.GetFromJsonAsync<Response<Wallpaper>>(url) ?? throw new InvalidOperationException("Failed to fetch wallpaper data.");

        if (!dbContext.Wallpapers.Any(w => w.Url == response.Data!.Url))
        {
            dbContext.Wallpapers.Add(new()
            {
                Url = response.Data!.Url,
                SourceUrl = response.Data!.SourceUrl,
                Author = response.Data!.Author,
                Uploader = response.Data!.Uploader,
                Type = "Bing",
            });

            dbContext.SaveChanges();
        }

        return Response<Wallpaper>.Success("OK", response.Data!);
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetToday()
    {
        string url = $"{upstreamUrl}today";
        Response<Wallpaper> response = await httpClient.GetFromJsonAsync<Response<Wallpaper>>(url) ?? throw new InvalidOperationException("Failed to fetch wallpaper data.");

        if (!dbContext.Wallpapers.Any(w => w.Url == response.Data!.Url))
        {
            dbContext.Wallpapers.Add(new()
            {
                Url = response.Data!.Url,
                SourceUrl = response.Data!.SourceUrl,
                Author = response.Data!.Author,
                Uploader = response.Data!.Uploader,
                Type = "Today",
            });

            dbContext.SaveChanges();
        }

        return Response<Wallpaper>.Success("OK", response.Data!);
    }
}
