namespace Snap.Hutao.Server.Model.Fufu;

public class FufuUidBanQuery
{
    public string Status { get; set; } = default!;

    public int TotalBanned { get; set; }

    public FufuUidBanItem[] BannedList { get; set; } = [];
}
