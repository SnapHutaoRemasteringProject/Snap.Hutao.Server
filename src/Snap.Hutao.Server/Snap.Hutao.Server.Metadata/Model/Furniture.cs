// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Furniture
{
    public JsonElement Types { get; set; } = default!;

    public uint SurfaceType { get; set; } = default!;

    public bool IsSpecial { get; set; } = default!;

    public uint SpecialType { get; set; } = default!;

    public uint Comfort { get; set; } = default!;

    public uint Cost { get; set; } = default!;

    public uint DiscountCost { get; set; } = default!;

    public bool CanFloat { get; set; } = default!;

    public bool IsUnique { get; set; } = default!;

    public string ItemIcon { get; set; } = default!;

    public uint RankLevel { get; set; } = default!;

    public uint GroupRecordType { get; set; } = default!;

    public JsonElement SourceTexts { get; set; } = default!;

    public uint Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public uint Rank { get; set; } = default!;
}
