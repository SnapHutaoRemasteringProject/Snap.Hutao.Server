// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Option;

public sealed class AppOptions
{
    public string JwtRaw { get; set; } = default!;

    public string RedisAddress { get; set; } = default!;
}
