// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_monsters")]
[PrimaryKey(nameof(DescribeId), nameof(Locale))]
public sealed class MetadataMonster
{
    public uint DescribeId { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Id { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(128)]
    public string? Title { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    public uint Type { get; set; }

    public uint Arkhe { get; set; }
}
