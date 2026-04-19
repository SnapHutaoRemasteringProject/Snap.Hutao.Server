// Copyright (c) Snap.HutaoRemasteringProject. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Wallpaper;

namespace Snap.Hutao.Server.API.Controller;

[Route("[controller]")]
[ApiController]
public class WallpaperController : ControllerBase
{
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
        string url = $"{upstreamUrl}hoyoplay";
        Response<Wallpaper> response = await httpClient.GetFromJsonAsync<Response<Wallpaper>>(url) ?? throw new InvalidOperationException("Failed to fetch wallpaper data.");

        if (!dbContext.Wallpapers.Any(w => w.Url == response.Data!.Url))
        {
            dbContext.Wallpapers.Add(new()
            {
                Url = response.Data!.Url,
                SourceUrl = response.Data!.SourceUrl,
                Author = response.Data!.Author,
                Uploader = response.Data!.Uploader,
                Type = "Hoyoplay",
            });

            dbContext.SaveChanges();
        }

        return Response<Wallpaper>.Success("OK", response.Data!);
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
