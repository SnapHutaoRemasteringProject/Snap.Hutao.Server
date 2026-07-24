// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Reliquary
{
    public JsonElement Ids { get; set; } = default!;

    public uint RankLevel { get; set; } = default!;

    public uint SetId { get; set; } = default!;

    public uint EquipType { get; set; } = default!;

    public uint ItemType { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Icon { get; set; } = default!;
}
