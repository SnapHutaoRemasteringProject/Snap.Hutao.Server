// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class AchievementGoal
{
    public uint Id { get; set; } = default!;

    public uint Order { get; set; } = default!;

    public string Name { get; set; } = default!;

    public JsonElement FinishReward { get; set; } = default!;

    public string Icon { get; set; } = default!;
}
