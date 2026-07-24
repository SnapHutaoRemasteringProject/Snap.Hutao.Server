// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class FurnitureSuite
{
    public uint Id { get; set; } = default!;

    public JsonElement Types { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string ItemIcon { get; set; } = default!;

    public JsonElement Units { get; set; } = default!;
}
