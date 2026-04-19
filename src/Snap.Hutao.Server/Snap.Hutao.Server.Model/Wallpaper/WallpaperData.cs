namespace Snap.Hutao.Server.Model.Wallpaper;

[Table("wallpaper")]
public class WallpaperData
{
    [Key]
    public int Id { get; set; }

    public string Url { get; set; } = default!;

    public string SourceUrl { get; set; } = default!;

    public string Author { get; set; } = default!;

    public string Uploader { get; set; } = default!;

    public string Type { get; set; } = default!;
}
