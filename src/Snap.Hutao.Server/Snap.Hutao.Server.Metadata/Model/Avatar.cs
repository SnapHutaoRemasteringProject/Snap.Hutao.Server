// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Avatar
{
    public uint Id { get; set; } = default!;

    public uint PromoteId { get; set; } = default!;

    public uint Sort { get; set; } = default!;

    public uint Body { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string SideIcon { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string BeginTime { get; set; } = default!;

    public uint Quality { get; set; } = default!;

    public uint Weapon { get; set; } = default!;

    public JsonElement BaseValue { get; set; } = default!;

    public JsonElement GrowCurves { get; set; } = default!;

    public JsonElement SkillDepot { get; set; } = default!;

    public JsonElement? FetterInfo { get; set; } = default!;

    public JsonElement? Costumes { get; set; } = default!;
}
