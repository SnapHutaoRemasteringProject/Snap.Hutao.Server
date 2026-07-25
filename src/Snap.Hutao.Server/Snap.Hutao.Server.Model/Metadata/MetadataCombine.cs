// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_combines")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataCombine
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Type { get; set; }

    public uint SubType { get; set; }

    public uint RecipeType { get; set; }

    public uint Cost { get; set; }

    [Column(TypeName = "json")]
    public string Result { get; set; } = default!;

    [Column(TypeName = "json")]
    public string Materials { get; set; } = default!;

    [Column(TypeName = "text")]
    public string? EffectDescription { get; set; }
}
