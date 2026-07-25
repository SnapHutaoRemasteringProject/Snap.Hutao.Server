// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Combine
{
    public uint Id { get; set; }

    public uint Type { get; set; }

    public uint SubType { get; set; }

    public uint RecipeType { get; set; }

    public uint Cost { get; set; }

    public JsonElement Result { get; set; }

    public JsonElement Materials { get; set; }

    public string? EffectDescription { get; set; }
}
