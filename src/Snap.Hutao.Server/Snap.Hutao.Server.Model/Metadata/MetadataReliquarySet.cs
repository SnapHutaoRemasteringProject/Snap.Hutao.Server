// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_reliquary_sets")]
[PrimaryKey(nameof(SetId), nameof(Locale))]
public sealed class MetadataReliquarySet
{
    public uint SetId { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    [Column(TypeName = "json")]
    public string EquipAffixIds { get; set; } = default!;

    [Column(TypeName = "json")]
    public string NeedNumber { get; set; } = default!;

    [Column(TypeName = "json")]
    public string Descriptions { get; set; } = default!;
}
