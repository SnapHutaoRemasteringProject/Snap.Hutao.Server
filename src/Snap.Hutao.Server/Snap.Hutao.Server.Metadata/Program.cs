// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Hosting;
using Quartz;
using Quartz.AspNetCore;
using Sentry;
using Sentry.AspNetCore;
using Snap.Hutao.Server.Metadata.Job;
using Snap.Hutao.Server.Metadata.Option;
using Snap.Hutao.Server.Metadata.Service;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity.Passport;
using Snap.Hutao.Server.Model.Response;

namespace Snap.Hutao.Server.Metadata;

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
                config.ScheduleJob<MetadataRefreshJob>(t => t.WithCronSchedule("0 0 4 * * ?"));
            })
            .AddQuartzServer(options => options.WaitForJobsToComplete = true)
            .AddResponseCompression()
            .AddSingleton(appOptions)
            .AddScoped<MetadataRefreshService>();

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
                    ValidIssuer = "metadata.snaphutaorp.org",
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

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("Metadata", new() { Version = "1.0", Title = "Metadata", Description = "元数据查询接口" });
        });

        WebApplication app = appBuilder.Build();

        EnsureDatabase(app);

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseResponseCompression();

        app.UseSwagger();
        app.UseSwaggerUI(option =>
        {
            option.RoutePrefix = "doc";
            option.DocumentTitle = "Hutao Metadata API";
            option.SwaggerEndpoint("/swagger/Metadata/swagger.json", "元数据");
            option.DefaultModelExpandDepth(-1);
            option.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });

        app.MapControllers();
        app.Run();
    }

    /// <summary>
    /// 主项目统一管理所有迁移，子项目启动时只检查数据库是否可用，不做迁移。
    /// </summary>
    private static void EnsureDatabase(IHost app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        MetadataDbContext metadataDbContext = scope.ServiceProvider.GetRequiredService<MetadataDbContext>();
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<IHost>>();

        EnsureCanConnect(appDbContext, logger);
        EnsureCanConnect(metadataDbContext, logger);
    }

    private static void EnsureCanConnect(DbContext context, ILogger logger)
    {
        if (context.Database.IsRelational())
        {
            if (!context.Database.CanConnect())
            {
                logger.LogWarning("[{Context}] 无法连接到数据库，迁移由主项目负责，此处不做自动创建",
                    context.GetType().Name);
            }
            else
            {
                logger.LogInformation("[{Context}] 数据库连接正常", context.GetType().Name);
            }
        }
    }
}
