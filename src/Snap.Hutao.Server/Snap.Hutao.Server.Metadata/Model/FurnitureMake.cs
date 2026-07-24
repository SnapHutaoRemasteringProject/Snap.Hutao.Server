// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class FurnitureMake
{
    public uint Id { get; set; } = default!;

    public uint ItemId { get; set; } = default!;

    public uint Experience { get; set; } = default!;

    public JsonElement Materials { get; set; } = default!;
}
