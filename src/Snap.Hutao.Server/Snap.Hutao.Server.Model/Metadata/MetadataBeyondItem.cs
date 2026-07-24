// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_beyond_items")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataBeyondItem
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    public uint Type { get; set; }

    public uint RankLevel { get; set; }
}
