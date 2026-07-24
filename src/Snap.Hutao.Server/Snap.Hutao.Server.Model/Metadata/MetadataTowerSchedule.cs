// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_tower_schedules")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataTowerSchedule
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [Column(TypeName = "datetime")]
    public DateTime Open { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Close { get; set; }

    [MaxLength(128)]
    public string? BuffName { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    [Column(TypeName = "json")]
    public string FloorIds { get; set; } = default!;

    [Column(TypeName = "json")]
    public string Descriptions { get; set; } = default!;
}
