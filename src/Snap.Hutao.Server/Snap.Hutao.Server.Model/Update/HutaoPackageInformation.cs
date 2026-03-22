namespace Snap.Hutao.Server.Model.Update;

public sealed class HutaoPackageInformation
{
    [JsonPropertyName("version")]
    public Version Version { get; set; } = default!;

    [JsonPropertyName("validation")]
    public string Validation { get; set; } = default!;

    [JsonPropertyName("mirrors")]
    public List<HutaoPackageMirror> Mirrors { get; set; } = default!;
}
