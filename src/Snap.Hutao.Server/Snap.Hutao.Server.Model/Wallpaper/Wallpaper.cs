namespace Snap.Hutao.Server.Model.Wallpaper;

public class Wallpaper
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("source_url")]
    public string SourceUrl { get; set; } = default!;

    [JsonPropertyName("author")]
    public string Author { get; set; } = default!;

    [JsonPropertyName("uploader")]
    public string Uploader { get; set; } = default!;
}
