// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_name_cards")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataNameCard
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public uint RankLevel { get; set; }

    [Column(TypeName = "json")]
    public string Pictures { get; set; } = default!;
}
