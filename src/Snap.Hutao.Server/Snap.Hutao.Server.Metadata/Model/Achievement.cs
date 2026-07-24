// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Achievement
{
    public uint Id { get; set; } = default!;

    public uint Goal { get; set; } = default!;

    public uint Order { get; set; } = default!;

    public uint PreviousId { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public JsonElement FinishReward { get; set; } = default!;

    public bool IsDeleteWatcherAfterFinish { get; set; } = default!;

    public uint Progress { get; set; } = default!;

    public string? Icon { get; set; } = default!;

    public string Version { get; set; } = default!;

    public bool IsDailyQuest { get; set; } = default!;
}
