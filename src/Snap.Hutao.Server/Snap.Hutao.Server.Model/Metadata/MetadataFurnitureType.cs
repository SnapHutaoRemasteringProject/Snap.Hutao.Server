// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_furniture_types")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataFurnitureType
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Category { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(128)]
    public string Name2 { get; set; } = default!;

    [MaxLength(256)]
    public string TabIcon { get; set; } = default!;

    public uint Sort { get; set; }
}
