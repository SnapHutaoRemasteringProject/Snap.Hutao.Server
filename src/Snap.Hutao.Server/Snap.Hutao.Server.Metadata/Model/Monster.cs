// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Monster
{
    public uint Id { get; set; } = default!;

    public uint DescribeId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Title { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public uint Type { get; set; } = default!;

    public uint Arkhe { get; set; } = default!;
}
