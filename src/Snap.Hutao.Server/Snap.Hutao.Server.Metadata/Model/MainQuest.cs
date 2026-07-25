// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class MainQuest
{
    public uint Id { get; set; }

    public uint Type { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

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

    public JsonElement RewardList { get; set; }
}
