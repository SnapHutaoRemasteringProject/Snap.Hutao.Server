// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_profile_pictures")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataProfilePicture
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Icon { get; set; }

    public uint UnlockType { get; set; }

    public uint UnlockParameter { get; set; }
}
