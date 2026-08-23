// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Fufu;

public class FufuUidBanItem
{
    [JsonPropertyName("uid")]
    public long Uid { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}
