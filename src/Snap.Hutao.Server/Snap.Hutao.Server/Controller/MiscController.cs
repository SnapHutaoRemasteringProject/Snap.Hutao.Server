// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Response;
using System.Net;

namespace Snap.Hutao.Server.Controller;

[ApiController]
[Route("")]
[ApiExplorerSettings(GroupName = "Misc")]
public class MiscController : ControllerBase
{
    private readonly AppDbContext appDbContext;

    public MiscController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpGet("patch/hutao")]
    public IActionResult GetPatchInfo()
    {
        // 获取新版本信息
        return Response<object>.Success("OK", new
        {
            validation = "",
            version = "1.0.0",
            mirrors = Array.Empty<string>(),
        });
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
        // 获取图片资源 重定向到wangdage12的服务器
        string baseUrl = "https://htserver.wdg.cloudns.ch/static/raw";
        return Redirect($"{baseUrl}/{category}/{fileName}");
    }

    [HttpGet("mgnt/am-i-banned")]
    public IActionResult CheckIfBanned()
    {
        // 检查游戏账户是否禁用注入，目前直接返回成功的响应即可
        return Response<object>.Success("OK", new { });
    }

    [HttpGet("tools")]
    public async Task<IActionResult> GetTools()
    {
        // 获取额外的第三方注入工具列表
        var tools = await appDbContext.Tools
            .Where(t => t.IsActive)
            .Select(t => new
            {
                t.Name,
                t.Description,
                t.Url,
                t.Version
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return Response<List<object>>.Success("OK", tools.Cast<object>().ToList());
    }

    [HttpGet("ip")]
    public IActionResult GetIpInformation()
    {
        string? ipAddress = null;

        if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
        {
            ipAddress = xForwardedFor.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault()?.Trim();
        }

        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        ipAddress ??= "Unknown";

        var division = "Unknown";

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
        string? ipAddress = null;
        if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
        {
            ipAddress = xForwardedFor.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault()?.Trim();
        }

        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        return ipAddress ?? "Unknown";
    }
}
