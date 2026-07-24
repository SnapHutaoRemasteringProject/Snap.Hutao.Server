// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class NameCard
{
    public uint Id { get; set; } = default!;

    public uint RankLevel { get; set; } = default!;

    public uint ItemType { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string? SpecialDescription { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public JsonElement Pictures { get; set; } = default!;
}
