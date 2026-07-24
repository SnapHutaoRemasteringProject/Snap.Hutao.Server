// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_chapters")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataChapter
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint GroupId { get; set; }

    [MaxLength(255)]
    public string? Number { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [MaxLength(255)]
    public string? ImageTitle { get; set; }

    public uint CityId { get; set; }
}
