// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_furniture_makes")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataFurnitureMake
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint ItemId { get; set; }

    public uint Experience { get; set; }

    [Column(TypeName = "json")]
    public string Materials { get; set; } = default!;
}
