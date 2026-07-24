// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_achievement_goals")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataAchievementGoal
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Order { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    [Column(TypeName = "json")]
    public string FinishReward { get; set; } = default!;
}
