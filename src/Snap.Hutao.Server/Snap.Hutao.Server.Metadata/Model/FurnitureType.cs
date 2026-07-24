// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class FurnitureType
{
    public uint Id { get; set; } = default!;

    public uint Category { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Name2 { get; set; } = default!;

    public string TabIcon { get; set; } = default!;

    public uint SceneType { get; set; } = default!;

    public bool BagPageOnly { get; set; } = default!;

    public bool IsShowInBag { get; set; } = default!;

    public uint Sort { get; set; } = default!;
}
