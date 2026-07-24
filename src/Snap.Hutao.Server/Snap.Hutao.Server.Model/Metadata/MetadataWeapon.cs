// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_weapons")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataWeapon
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public uint RankLevel { get; set; }

    public uint WeaponType { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    [MaxLength(256)]
    public string AwakenIcon { get; set; } = default!;

    [Column(TypeName = "json")]
    public string GrowCurves { get; set; } = default!;

    [Column(TypeName = "json")]
    public string? Affix { get; set; }

    [Column(TypeName = "json")]
    public string CultivationItems { get; set; } = default!;
}
