// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class GachaEvent
{
    public string Name { get; set; } = default!;

    public string Version { get; set; } = default!;

    public int Order { get; set; } = default!;

    public string Banner { get; set; } = default!;

    public string Banner2 { get; set; } = default!;

    public string From { get; set; } = default!;

    public string To { get; set; } = default!;

    public int Type { get; set; } = default!;

    public JsonElement UpOrangeList { get; set; } = default!;

    public JsonElement UpPurpleList { get; set; } = default!;
}
