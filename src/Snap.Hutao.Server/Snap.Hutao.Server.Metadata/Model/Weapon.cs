// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Weapon
{
    public uint Id { get; set; } = default!;

    public uint PromoteId { get; set; } = default!;

    public uint Sort { get; set; } = default!;

    public uint WeaponType { get; set; } = default!;

    public uint RankLevel { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string AwakenIcon { get; set; } = default!;

    public JsonElement GrowCurves { get; set; } = default!;

    public JsonElement? Affix { get; set; } = default!;

    public JsonElement CultivationItems { get; set; } = default!;
}
