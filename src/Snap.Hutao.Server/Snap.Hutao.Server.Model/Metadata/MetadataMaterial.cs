// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_materials")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataMaterial
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [MaxLength(128)]
    public string? TypeDescription { get; set; }

    public uint RankLevel { get; set; }

    public uint ItemType { get; set; }

    public uint MaterialType { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }
}
