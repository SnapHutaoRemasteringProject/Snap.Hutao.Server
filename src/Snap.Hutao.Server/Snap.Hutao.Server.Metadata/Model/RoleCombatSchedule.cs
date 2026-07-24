// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class RoleCombatSchedule
{
    public uint Id { get; set; } = default!;

    public string Begin { get; set; } = default!;

    public string End { get; set; } = default!;

    public JsonElement Elements { get; set; } = default!;

    public JsonElement SpecialAvatars { get; set; } = default!;

    public JsonElement InitialAvatars { get; set; } = default!;
}
