// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity.Passport;
using Snap.Hutao.Server.Model.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Snap.Hutao.Server.Controller;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "WebApi")]
public class WebApiController : ControllerBase
{
    private readonly AppDbContext appDbContext;
    private readonly IConfiguration configuration;

    public WebApiController(AppDbContext appDbContext, IConfiguration configuration)
    {
        this.appDbContext = appDbContext;
        this.configuration = configuration;
    }

    [HttpPost("web-api/login")]
    public async Task<IActionResult> WebApiLogin([FromBody] WebApiLoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return Model.Response.Response.Fail(ReturnCode.InvalidRequestBody, "Email and password are required");
        }

        // 验证用户凭据
        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.NormalizedUserName == request.Email.ToUpperInvariant())
            .ConfigureAwait(false);

        if (user == null || !await VerifyPasswordAsync(user, request.Password))
        {
            return Model.Response.Response.Fail(ReturnCode.LoginFail, "Invalid email or password");
        }

        // 检查用户是否具有维护者权限
        if (!user.IsMaintainer)
        {
            return Model.Response.Response.Fail(ReturnCode.LoginFail, "Insufficient permissions");
        }

        // 创建token
        var token = GenerateJwtToken(user);

        return Response<object>.Success("success", new
        {
            access_token = token,
            expires_in = 3600 * 24 // 24小时
        });
    }

    private async Task<bool> VerifyPasswordAsync(HutaoUser user, string password)
    {
        var passwordHasher = new PasswordHasher<HutaoUser>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

    private string GenerateJwtToken(HutaoUser user)
    {
        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            jwtKey = "default_jwt_key_for_development_only_change_in_production";
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("IsMaintainer", user.IsMaintainer.ToString()),
            new Claim("IsLicensedDeveloper", user.IsLicensedDeveloper.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"] ?? "homa.snaphutaorp.org",
            audience: configuration["Jwt:Audience"] ?? "web_api",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class WebApiLoginRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
