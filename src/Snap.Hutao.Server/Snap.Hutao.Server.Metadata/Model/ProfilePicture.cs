// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class ProfilePicture
{
    public uint Id { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string Name { get; set; } = default!;

    public uint UnlockType { get; set; } = default!;

    public uint UnlockParameter { get; set; } = default!;
}
