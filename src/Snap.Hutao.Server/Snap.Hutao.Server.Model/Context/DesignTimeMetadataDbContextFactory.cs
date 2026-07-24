// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Design;

namespace Snap.Hutao.Server.Model.Context;

public sealed class DesignTimeMetadataDbContextFactory : IDesignTimeDbContextFactory<MetadataDbContext>
{
    public MetadataDbContext CreateDbContext(string[] args)
    {
        string basePath = Path.GetDirectoryName(typeof(DesignTimeMetadataDbContextFactory).Assembly.Location)!;
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        DbContextOptionsBuilder<MetadataDbContext> builder = new();
        string connectionString = configuration.GetConnectionString("MetadataMysql8")!;
        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new MetadataDbContext(builder.Options);
    }
}
