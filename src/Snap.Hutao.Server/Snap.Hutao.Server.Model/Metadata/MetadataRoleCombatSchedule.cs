// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Metadata;

[Table("metadata_role_combat_schedules")]
[PrimaryKey(nameof(Id), nameof(Locale))]
public sealed class MetadataRoleCombatSchedule
{
    public uint Id { get; set; }

    [MaxLength(16)]
    public string Locale { get; set; } = default!;

    [Column(TypeName = "datetime")]
    public DateTime Begin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime End { get; set; }

    [Column(TypeName = "json")]
    public string Elements { get; set; } = default!;

    [Column(TypeName = "json")]
    public string SpecialAvatars { get; set; } = default!;

    [Column(TypeName = "json")]
    public string InitialAvatars { get; set; } = default!;
}
