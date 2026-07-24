// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_reliquaries")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataReliquary
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint SetId { get; set; }

    public uint EquipType { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public uint RankLevel { get; set; }

    [Column(TypeName = "json")]
    public string Ids { get; set; } = default!;
}
