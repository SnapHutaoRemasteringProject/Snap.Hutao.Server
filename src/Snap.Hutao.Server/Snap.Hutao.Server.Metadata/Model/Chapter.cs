// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Metadata.Model;

public sealed class Chapter
{
    public uint Id { get; set; } = default!;

    public uint GroupId { get; set; } = default!;

    public uint BeginQuestId { get; set; } = default!;

    public uint EndQuestId { get; set; } = default!;

    public uint NeedPlayerLevel { get; set; } = default!;

    public string Number { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string ImageTitle { get; set; } = default!;

    public string SerialNumberIcon { get; set; } = default!;

    public uint CityId { get; set; } = default!;

    public uint QuestType { get; set; } = default!;
}
