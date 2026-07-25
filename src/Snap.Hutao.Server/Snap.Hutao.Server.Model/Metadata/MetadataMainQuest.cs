// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_main_quests")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataMainQuest
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    public uint Type { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column(TypeName = "text")]
    public string? UnlockDescription { get; set; }

    public uint ChapterId { get; set; }

    public uint SortWeight { get; set; }

    public uint RecommendLevel { get; set; }

    public uint ActivityId { get; set; }

    public uint MainQuestTag { get; set; }

    public uint ShowType { get; set; }

    public bool Repeatable { get; set; }

    public uint Series { get; set; }

    public uint TaskId { get; set; }

    [Column(TypeName = "json")]
    public string RewardList { get; set; } = default!;
}
