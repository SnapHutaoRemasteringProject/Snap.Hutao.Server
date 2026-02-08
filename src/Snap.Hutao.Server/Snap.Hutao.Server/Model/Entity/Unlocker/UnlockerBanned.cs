namespace Snap.Hutao.Server.Model.Entity.Unlocker;

[Table("unlocker_banned")]
public class UnlockerBanned
{
    [Key]
    [StringLength(10, MinimumLength = 9)]
    public string Uid { get; set; } = default!;
}
