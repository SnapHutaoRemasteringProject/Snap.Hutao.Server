// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class ReliquarySet
{
    public uint SetId { get; set; } = default!;

    public uint EquipAffixId { get; set; } = default!;

    public JsonElement EquipAffixIds { get; set; } = default!;

    public JsonElement NeedNumber { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public JsonElement Descriptions { get; set; } = default!;
}
