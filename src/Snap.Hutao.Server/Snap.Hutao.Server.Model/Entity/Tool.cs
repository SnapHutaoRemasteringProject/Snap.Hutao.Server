// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Server.Model.Entity;

[Table("tools")]
public class Tool
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Url { get; set; } = default!;

    public string Version { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
