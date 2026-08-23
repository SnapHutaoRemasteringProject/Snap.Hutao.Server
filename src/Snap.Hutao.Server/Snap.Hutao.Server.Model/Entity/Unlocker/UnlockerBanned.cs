// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Entity.Unlocker;

[Table("unlocker_banned")]
public class UnlockerBanned
{
    [Key]
    [StringLength(10, MinimumLength = 9)]
    public string Uid { get; set; } = default!;

    public string Reason { get; set; } = "You was banned by operator";
}
