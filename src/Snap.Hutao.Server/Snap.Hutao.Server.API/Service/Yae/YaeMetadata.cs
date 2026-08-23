// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Service.Yae;

internal sealed class YaeMetadata
{
    public string Version { get; set; } = string.Empty;

    public uint Id { get; set; }

    public uint Status { get; set; }

    public uint TotalProgress { get; set; }

    public uint CurrentProgress { get; set; }

    public uint FinishTimestamp { get; set; }

    public uint StoreCmdId { get; set; }

    public uint AchievementCmdId { get; set; }

    public Dictionary<uint, MethodRvaConfig> MethodRva { get; } = [];
}
