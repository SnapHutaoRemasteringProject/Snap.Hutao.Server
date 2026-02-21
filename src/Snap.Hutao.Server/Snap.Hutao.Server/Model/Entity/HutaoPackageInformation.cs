// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Entity;

[Table("hutao_package_informations")]
public class HutaoPackageInformation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Version { get; set; } = default!;

    [Required]
    [StringLength(128)]
    public string Validation { get; set; } = default!;

    [Required]
    [StringLength(500)]
    public string Url { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string MirrorName { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string MirrorType { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
