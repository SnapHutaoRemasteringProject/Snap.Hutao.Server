// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Simpl;
using Snap.Hutao.Server.API.Controller.Filter;
using Snap.Hutao.Server.API.Option;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity.Passport;
using Snap.Hutao.Server.Model.Response;

namespace Snap.Hutao.Server.API;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

        appBuilder.WebHost.UseSentry(options =>
        {
            options.Release = $"{DateTimeOffset.Now:yyyy.M.d.Hm}";
            options.Dsn = "https://7de19654a539bfdd56a798ce89e85137@host.docker.internal:9510/7";
            options.TracesSampleRate = 1D;
            options.SendDefaultPii = true;
        });

        appBuilder.Services.AddSentryTunneling();

        IServiceCollection services = appBuilder.Services;

        AppOptions appOptions = appBuilder.Configuration.GetSection("App").Get<AppOptions>()!;

        // Services
        services
            .AddAuthorization()
            .AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()))
            .AddDbContextPool<AppDbContext>((serviceProvider, options) =>
            {
                string connectionString = appBuilder.Configuration.GetConnectionString("PrimaryMysql8")!;
                serviceProvider
                    .GetRequiredService<ILogger<AppDbContext>>()
                    .LogInformation("AppDbContext Using connection string: [{Constr}]", connectionString);

                options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));
            })
            .AddDbContextPool<MetadataDbContext>((serviceProvider, options) =>
            {
                string connectionString = appBuilder.Configuration.GetConnectionString("MetadataMysql8")!;
                serviceProvider
                    .GetRequiredService<ILogger<MetadataDbContext>>()
                    .LogInformation("MetadataDbContext Using connection string: [{Constr}]", connectionString);

                options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));
            })
            .AddEndpointsApiExplorer()
            .AddHttpClient()
            .AddMemoryCache()
            .AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions["retcode"] = context.ProblemDetails.Status ?? (int)ReturnCode.InternalStateException;
                    context.ProblemDetails.Extensions["message"] = context.Exception?.Message ?? string.Empty;
                };
            })
            .AddQuartz(config =>
            {
                config.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
            })
            .AddQuartzServer(options => options.WaitForJobsToComplete = true)
            .AddResponseCompression()

            // .AddSingleton<DiscordService>()
            .AddSingleton(appOptions)
            .AddTransient<ValidateMaintainPermission>();

        // Authentication
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = appOptions.GetJwtSecurityKey(),
                    ValidateIssuer = true,
                    ValidIssuer = "api.snaphutaorp.org",
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(10),
                };
            });

        // Identity
        services
            .AddIdentityCore<HutaoUser>(options =>
            {
                PasswordOptions passwordOptions = options.Password;

                passwordOptions.RequiredLength = 8;
                passwordOptions.RequiredUniqueChars = 0;
                passwordOptions.RequireLowercase = false;
                passwordOptions.RequireUppercase = false;
                passwordOptions.RequireNonAlphanumeric = false;
                passwordOptions.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        // Mvc
        services
            .AddControllers(options =>
            {
                // Disable non-nullable as [Required]
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .AddJsonOptions(options =>
            {
                JsonSerializerOptions jsonOptions = options.JsonSerializerOptions;

                jsonOptions.PropertyNamingPolicy = null;
                jsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                jsonOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                jsonOptions.PropertyNameCaseInsensitive = true;
                jsonOptions.WriteIndented = true;
            });

        WebApplication app = appBuilder.Build();

        // 中间件的顺序敏感
        // https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/#middleware-order
        // ExceptionHandler
        app.UseExceptionHandler();

        // HSTS
        // HttpsRedirection
        app.UseHttpsRedirection();

        // Static Files
        app.UseDefaultFiles(new DefaultFilesOptions()
        {
            DefaultFileNames =
            {
                "index.html",
            },
        });
        app.UseStaticFiles();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });

        // Routes
        // CORS
        app.UseCors();

        // Authentication
        app.UseAuthentication();

        // Authorization
        app.UseAuthorization();

        // Custom
        app.UseResponseCompression();

        // Endpoint
        app.MapControllers();

        app.Run();
    }
}
