// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class TowerSchedule
{
    public uint Id { get; set; } = default!;

    public JsonElement FloorIds { get; set; } = default!;

    public string Open { get; set; } = default!;

    public string Close { get; set; } = default!;

    public string BuffName { get; set; } = default!;

    public JsonElement Descriptions { get; set; } = default!;

    public string Icon { get; set; } = default!;
}
