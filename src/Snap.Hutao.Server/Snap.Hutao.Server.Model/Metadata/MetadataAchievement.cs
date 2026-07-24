// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_achievements")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataAchievement
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Goal { get; set; }

    public uint Order { get; set; }

    [MaxLength(256)]
    public string? Title { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public uint Progress { get; set; }

    [MaxLength(32)]
    public string Version { get; set; } = default!;

    public bool IsDailyQuest { get; set; }

    public uint PreviousId { get; set; }

    [Column(TypeName = "json")]
    public string FinishReward { get; set; } = default!;
}
