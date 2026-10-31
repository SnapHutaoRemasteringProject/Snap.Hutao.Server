// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Option;

public static class AppOptionsExtension
{
    public static SymmetricSecurityKey GetJwtSecurityKey(this AppOptions options)
    {
        return new(Encoding.UTF8.GetBytes(options.JwtRaw));
    }

    public static string GetRawBaseUrl(this AppOptions options)
    {
        return GetRequiredBaseUrl(options.StaticRawBaseUrl, nameof(AppOptions.StaticRawBaseUrl));
    }

    public static string GetZipBaseUrl(this AppOptions options)
    {
        return GetRequiredBaseUrl(options.StaticZipBaseUrl, nameof(AppOptions.StaticZipBaseUrl));
    }

    public static string GetLowRawBaseUrl(this AppOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.StaticLowRawBaseUrl))
        {
            return options.StaticLowRawBaseUrl.TrimEnd('/');
        }

        return $"{GetParentDirectory(options.GetRawBaseUrl())}/low";
    }

    public static string GetLowZipBaseUrl(this AppOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.StaticLowZipBaseUrl))
        {
            return options.StaticLowZipBaseUrl.TrimEnd('/');
        }

        return $"{GetParentDirectory(options.GetZipBaseUrl())}/low_zip";
    }

    public static string GetStaticSizeUrl(this AppOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.StaticSizeUrl))
        {
            return options.StaticSizeUrl.TrimEnd('/');
        }

        return $"{GetParentDirectory(options.GetZipBaseUrl())}/size.json";
    }

    private static string GetParentDirectory(string url)
    {
        int lastSeparator = url.LastIndexOf('/');
        return lastSeparator < 0 ? string.Empty : url[..lastSeparator];
    }

    private static string GetRequiredBaseUrl(string? baseUrl, string name)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException($"The static resource configuration '{name}' is missing.", name);
        }

        return baseUrl.TrimEnd('/');
    }
}
