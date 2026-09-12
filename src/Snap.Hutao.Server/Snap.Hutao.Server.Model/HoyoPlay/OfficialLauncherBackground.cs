// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.HoyoPlay;

public sealed class OfficialLauncherBackground
{
    [JsonPropertyName("retcode")]
    public int Retcode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("data")]
    public OfficialLauncherBackgroundData? Data { get; set; }
}

[SuppressMessage("", "SA1402")]
public sealed class OfficialLauncherBackgroundData
{
    [JsonPropertyName("game_info_list")]
    public List<OfficialLauncherGameInfo>? GameInfoList { get; set; }
}

[SuppressMessage("", "SA1402")]
public sealed class OfficialLauncherGameInfo
{
    [JsonPropertyName("game")]
    public OfficialLauncherGame? Game { get; set; }

    [JsonPropertyName("backgrounds")]
    public List<OfficialLauncherBackgroundItem>? Backgrounds { get; set; }
}

[SuppressMessage("", "SA1402")]
public sealed class OfficialLauncherGame
{
    [JsonPropertyName("biz")]
    public string? Biz { get; set; }
}

[SuppressMessage("", "SA1402")]
public sealed class OfficialLauncherBackgroundItem
{
    [JsonPropertyName("background")]
    public OfficialLauncherBackgroundImage? Background { get; set; }
}

[SuppressMessage("", "SA1402")]
public sealed class OfficialLauncherBackgroundImage
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
