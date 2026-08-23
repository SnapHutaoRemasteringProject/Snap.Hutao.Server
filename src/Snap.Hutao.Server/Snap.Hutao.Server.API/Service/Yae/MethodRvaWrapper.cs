// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Service.Yae;

public sealed class MethodRvaWrapper
{
    [JsonPropertyName("chinese")]
    public required MethodRva Chinese { get; init; }

    [JsonPropertyName("oversea")]
    public required MethodRva Oversea { get; init; }
}
