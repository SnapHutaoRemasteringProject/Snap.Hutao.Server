// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_hyper_link_names")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataHyperLinkName
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(256)]
    public string? Name { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }
}
