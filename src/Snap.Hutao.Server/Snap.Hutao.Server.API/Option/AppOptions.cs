// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.


// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Option;

public sealed class AppOptions
{
    public string JwtRaw { get; set; } = default!;

    public string RedisAddress { get; set; } = default!;

    public string StaticRawBaseUrl { get; set; } = default!;
}
