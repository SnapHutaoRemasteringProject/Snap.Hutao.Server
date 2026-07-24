// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using LibGit2Sharp;
using Snap.Hutao.Server.Extension;
using Snap.Hutao.Server.Metadata.Model;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Metadata;

namespace Snap.Hutao.Server.Metadata.Service;

public sealed class MetadataRefreshService
{
    private const string RepoDirectory = "Snap.Metadata";

    private static readonly string[] Locales =
    [
        "CHS", "CHT", "EN", "JP", "KR", "DE", "FR", "ES", "PT", "RU", "TH", "VI", "ID", "IT", "TR",
    ];

    private readonly ILogger<MetadataRefreshService> logger;
    private readonly AppDbContext appDbContext;
    private readonly MetadataDbContext metadataDbContext;

    public MetadataRefreshService(
        ILogger<MetadataRefreshService> logger,
        AppDbContext appDbContext,
        MetadataDbContext metadataDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
        this.metadataDbContext = metadataDbContext;
    }

    public async Task RefreshAllAsync(CancellationToken cancellationToken = default)
    {
        await EnsureGitRepo(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("开始全量刷新元数据（15种语言）");

        foreach (string locale in Locales)
        {
            logger.LogInformation("正在处理语言: {Locale}", locale);
            await RefreshByLocaleAsync(locale, cancellationToken).ConfigureAwait(false);
        }

        await RefreshKnownItemsAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("全量刷新元数据完成");
    }

    private async Task RefreshByLocaleAsync(string locale, CancellationToken cancellationToken)
    {
        string localePath = Path.Combine(RepoDirectory, "Genshin", locale);

        if (!Directory.Exists(localePath))
        {
            logger.LogWarning("语言目录不存在: {Path}", localePath);
            return;
        }

        await RefreshTableAsync(locale, localePath, "Achievement.json", typeof(MetadataAchievement),
            () => metadataDbContext.Achievements.Where(a => a.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Achievement>>(json);
                return items?.Select(a => new MetadataAchievement
                {
                    Id = a.Id,
                    Locale = loc,
                    Goal = a.Goal,
                    Order = a.Order,
                    Title = a.Title,
                    Description = a.Description,
                    Progress = a.Progress,
                    Version = a.Version,
                    IsDailyQuest = a.IsDailyQuest,
                    PreviousId = a.PreviousId,
                    FinishReward = a.FinishReward.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "AchievementGoal.json", typeof(MetadataAchievementGoal),
            () => metadataDbContext.AchievementGoals.Where(a => a.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<AchievementGoal>>(json);
                return items?.Select(a => new MetadataAchievementGoal
                {
                    Id = a.Id,
                    Locale = loc,
                    Order = a.Order,
                    Name = a.Name,
                    Icon = a.Icon,
                    FinishReward = a.FinishReward.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "BeyondItem.json", typeof(MetadataBeyondItem),
            () => metadataDbContext.BeyondItems.Where(b => b.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<BeyondItem>>(json);
                return items?.Select(b => new MetadataBeyondItem
                {
                    Id = b.Id,
                    Locale = loc,
                    Name = b.Name,
                    Description = b.Description,
                    Type = b.Type,
                    RankLevel = b.RankLevel,
                    Icon = b.Icon,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Chapter.json", typeof(MetadataChapter),
            () => metadataDbContext.Chapters.Where(c => c.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Chapter>>(json);
                return items?.Select(c => new MetadataChapter
                {
                    Id = c.Id,
                    Locale = loc,
                    GroupId = c.GroupId,
                    Number = string.IsNullOrEmpty(c.Number) ? null : c.Number,
                    Title = string.IsNullOrEmpty(c.Title) ? null : c.Title,
                    ImageTitle = string.IsNullOrEmpty(c.ImageTitle) ? null : c.ImageTitle,
                    CityId = c.CityId,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "DisplayItem.json", typeof(MetadataDisplayItem),
            () => metadataDbContext.DisplayItems.Where(d => d.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<DisplayItem>>(json);
                return items?.Select(d => new MetadataDisplayItem
                {
                    Id = d.Id,
                    Locale = loc,
                    Name = d.Name,
                    Description = d.Description,
                    TypeDescription = d.TypeDescription,
                    RankLevel = d.RankLevel,
                    ItemType = d.ItemType,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Furniture.json", typeof(MetadataFurniture),
            () => metadataDbContext.Furniture.Where(f => f.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Furniture>>(json);
                return items?.Select(f => new MetadataFurniture
                {
                    Id = f.Id,
                    Locale = loc,
                    Name = f.Name,
                    Description = f.Description,
                    Comfort = f.Comfort,
                    Cost = f.Cost,
                    RankLevel = f.RankLevel,
                    Types = f.Types.GetRawText(),
                    SourceTexts = f.SourceTexts.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "FurnitureMake.json", typeof(MetadataFurnitureMake),
            () => metadataDbContext.FurnitureMakes.Where(f => f.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<FurnitureMake>>(json);
                return items?.Select(f => new MetadataFurnitureMake
                {
                    Id = f.Id,
                    Locale = loc,
                    ItemId = f.ItemId,
                    Experience = f.Experience,
                    Materials = f.Materials.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "FurnitureSuite.json", typeof(MetadataFurnitureSuite),
            () => metadataDbContext.FurnitureSuites.Where(f => f.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<FurnitureSuite>>(json);
                return items?.Select(f => new MetadataFurnitureSuite
                {
                    Id = f.Id,
                    Locale = loc,
                    Name = f.Name,
                    Description = f.Description,
                    Types = f.Types.GetRawText(),
                    Units = f.Units.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "FurnitureType.json", typeof(MetadataFurnitureType),
            () => metadataDbContext.FurnitureTypes.Where(f => f.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<FurnitureType>>(json);
                return items?.Select(f => new MetadataFurnitureType
                {
                    Id = f.Id,
                    Locale = loc,
                    Category = f.Category,
                    Name = f.Name,
                    Name2 = f.Name2,
                    TabIcon = f.TabIcon,
                    Sort = f.Sort,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "HyperLinkName.json", typeof(MetadataHyperLinkName),
            () => metadataDbContext.HyperLinkNames.Where(h => h.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<HyperLinkName>>(json);
                return items?.Select(h => new MetadataHyperLinkName
                {
                    Id = h.Id,
                    Locale = loc,
                    Name = h.Name,
                    Description = h.Description,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Material.json", typeof(MetadataMaterial),
            () => metadataDbContext.Materials.Where(m => m.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Material>>(json);
                return items?.Select(m => new MetadataMaterial
                {
                    Id = m.Id,
                    Locale = loc,
                    Name = m.Name,
                    Description = m.Description,
                    TypeDescription = m.TypeDescription,
                    RankLevel = m.RankLevel,
                    ItemType = m.ItemType,
                    MaterialType = m.MaterialType,
                    Icon = m.Icon,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Monster.json", typeof(MetadataMonster),
            () => metadataDbContext.Monsters.Where(m => m.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Monster>>(json);
                return items?.Select(m => new MetadataMonster
                {
                    Id = m.Id,
                    Locale = loc,
                    DescribeId = m.DescribeId,
                    Name = m.Name,
                    Title = m.Title,
                    Icon = m.Icon,
                    Type = m.Type,
                    Arkhe = m.Arkhe,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "NameCard.json", typeof(MetadataNameCard),
            () => metadataDbContext.NameCards.Where(n => n.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<NameCard>>(json);
                return items?.Select(n => new MetadataNameCard
                {
                    Id = n.Id,
                    Locale = loc,
                    Name = n.Name,
                    Description = n.Description,
                    RankLevel = n.RankLevel,
                    Pictures = n.Pictures.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "ProfilePicture.json", typeof(MetadataProfilePicture),
            () => metadataDbContext.ProfilePictures.Where(p => p.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<ProfilePicture>>(json);
                return items?.Select(p => new MetadataProfilePicture
                {
                    Id = p.Id,
                    Locale = loc,
                    Name = p.Name,
                    Icon = p.Icon,
                    UnlockType = p.UnlockType,
                    UnlockParameter = p.UnlockParameter,
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Reliquary.json", typeof(MetadataReliquary),
            () => metadataDbContext.Reliquaries.Where(r => r.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Reliquary>>(json);
                return items?.SelectMany(r =>
                {
                    var ids = r.Ids.Deserialize<uint[]>();
                    return (ids ?? []).Select(id => new MetadataReliquary
                    {
                        Id = id,
                        Locale = loc,
                        SetId = r.SetId,
                        EquipType = r.EquipType,
                        Name = r.Name,
                        Description = r.Description,
                        RankLevel = r.RankLevel,
                        Ids = r.Ids.GetRawText(),
                    });
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "ReliquarySet.json", typeof(MetadataReliquarySet),
            () => metadataDbContext.ReliquarySets.Where(r => r.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<ReliquarySet>>(json);
                return items?.Select(r => new MetadataReliquarySet
                {
                    SetId = r.SetId,
                    Locale = loc,
                    Name = r.Name,
                    Icon = r.Icon,
                    EquipAffixIds = r.EquipAffixIds.GetRawText(),
                    NeedNumber = r.NeedNumber.GetRawText(),
                    Descriptions = r.Descriptions.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "RoleCombatSchedule.json", typeof(MetadataRoleCombatSchedule),
            () => metadataDbContext.RoleCombatSchedules.Where(r => r.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<RoleCombatSchedule>>(json);
                return items?.Select(r => new MetadataRoleCombatSchedule
                {
                    Id = r.Id,
                    Locale = loc,
                    Begin = DateTime.Parse(r.Begin),
                    End = DateTime.Parse(r.End),
                    Elements = r.Elements.GetRawText(),
                    SpecialAvatars = r.SpecialAvatars.GetRawText(),
                    InitialAvatars = r.InitialAvatars.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "TowerSchedule.json", typeof(MetadataTowerSchedule),
            () => metadataDbContext.TowerSchedules.Where(t => t.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<TowerSchedule>>(json);
                return items?.Select(t => new MetadataTowerSchedule
                {
                    Id = t.Id,
                    Locale = loc,
                    Open = DateTime.Parse(t.Open),
                    Close = DateTime.Parse(t.Close),
                    BuffName = t.BuffName,
                    Icon = t.Icon,
                    FloorIds = t.FloorIds.GetRawText(),
                    Descriptions = t.Descriptions.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        await RefreshTableAsync(locale, localePath, "Weapon.json", typeof(MetadataWeapon),
            () => metadataDbContext.Weapons.Where(w => w.Locale == locale), cancellationToken,
            (json, loc) =>
            {
                var items = JsonSerializer.Deserialize<List<Weapon>>(json);
                return items?.Select(w => new MetadataWeapon
                {
                    Id = w.Id,
                    Locale = loc,
                    Name = w.Name,
                    Description = w.Description,
                    RankLevel = w.RankLevel,
                    WeaponType = w.WeaponType,
                    Icon = w.Icon,
                    AwakenIcon = w.AwakenIcon,
                    GrowCurves = w.GrowCurves.GetRawText(),
                    Affix = w.Affix?.GetRawText(),
                    CultivationItems = w.CultivationItems.GetRawText(),
                }).ToList();
            }).ConfigureAwait(false);

        // Avatars are in a scattered directory
        await RefreshAvatarsAsync(locale, localePath, cancellationToken).ConfigureAwait(false);

        // GachaEvents
        await RefreshGachaEventsAsync(locale, localePath, cancellationToken).ConfigureAwait(false);
    }

    private async Task RefreshAvatarsAsync(string locale, string localePath, CancellationToken cancellationToken)
    {
        string avatarDir = Path.Combine(localePath, "Avatar");
        if (!Directory.Exists(avatarDir))
        {
            return;
        }

        try
        {
            var entities = new List<MetadataAvatar>();

            foreach (string file in Directory.GetFiles(avatarDir, "*.json"))
            {
                string json = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
                var avatar = JsonSerializer.Deserialize<Avatar>(json);
                if (avatar != null)
                {
                    entities.Add(new MetadataAvatar
                    {
                        Id = avatar.Id,
                        Locale = locale,
                        Name = avatar.Name,
                        Description = avatar.Description,
                        Quality = avatar.Quality,
                        Weapon = avatar.Weapon,
                        Icon = avatar.Icon,
                        SideIcon = avatar.SideIcon,
                        Sort = avatar.Sort,
                        Body = avatar.Body,
                        BaseValue = avatar.BaseValue.GetRawText(),
                        GrowCurves = avatar.GrowCurves.GetRawText(),
                        SkillDepot = avatar.SkillDepot.GetRawText(),
                        FetterInfo = avatar.FetterInfo?.GetRawText(),
                        Costumes = avatar.Costumes?.GetRawText(),
                    });
                }
            }

            if (entities.Count > 0)
            {
                await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await metadataDbContext.Avatars.Where(a => a.Locale == locale).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
                    await metadataDbContext.Avatars.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                    await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                    logger.LogInformation("[{Locale}] avatars: {Count} 条", locale, entities.Count);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Locale}] 刷新 avatars 失败", locale);
        }
    }

    private async Task RefreshGachaEventsAsync(string locale, string localePath, CancellationToken cancellationToken)
    {
        try
        {
            string filePath = Path.Combine(localePath, "GachaEvent.json");
            if (!File.Exists(filePath))
            {
                return;
            }

            string json = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);
            var events = JsonSerializer.Deserialize<List<GachaEvent>>(json);
            if (events == null || events.Count == 0)
            {
                return;
            }

            var entities = events.Select(e => new GachaEventInfo
            {
                Version = e.Version,
                Name = e.Name,
                Locale = locale,
                Order = (uint)e.Order,
                From = DateTime.Parse(e.From),
                To = DateTime.Parse(e.To),
                Type = (GachaConfigType)e.Type,
            }).ToList();

            // Fill UpOrangeList/UpPurpleList for each event
            for (int i = 0; i < events.Count; i++)
            {
                var e = events[i];
                var entity = entities[i];
                var orangeIds = e.UpOrangeList.Deserialize<uint[]>() ?? [];
                var purpleIds = e.UpPurpleList.Deserialize<uint[]>() ?? [];

                for (int j = 0; j < Math.Min(orangeIds.Length, 16); j++)
                {
                    switch (j)
                    {
                        case 0: entity.UpOrangeItem1 = orangeIds[j]; break;
                        case 1: entity.UpOrangeItem2 = orangeIds[j]; break;
                        case 2: entity.UpOrangeItem3 = orangeIds[j]; break;
                        case 3: entity.UpOrangeItem4 = orangeIds[j]; break;
                        case 4: entity.UpOrangeItem5 = orangeIds[j]; break;
                        case 5: entity.UpOrangeItem6 = orangeIds[j]; break;
                        case 6: entity.UpOrangeItem7 = orangeIds[j]; break;
                        case 7: entity.UpOrangeItem8 = orangeIds[j]; break;
                        case 8: entity.UpOrangeItem9 = orangeIds[j]; break;
                        case 9: entity.UpOrangeItem10 = orangeIds[j]; break;
                        case 10: entity.UpOrangeItem11 = orangeIds[j]; break;
                        case 11: entity.UpOrangeItem12 = orangeIds[j]; break;
                        case 12: entity.UpOrangeItem13 = orangeIds[j]; break;
                        case 13: entity.UpOrangeItem14 = orangeIds[j]; break;
                        case 14: entity.UpOrangeItem15 = orangeIds[j]; break;
                        case 15: entity.UpOrangeItem16 = orangeIds[j]; break;
                    }
                }

                for (int j = 0; j < Math.Min(purpleIds.Length, 5); j++)
                {
                    switch (j)
                    {
                        case 0: entity.UpPurpleItem1 = purpleIds[j]; break;
                        case 1: entity.UpPurpleItem2 = purpleIds[j]; break;
                        case 2: entity.UpPurpleItem3 = purpleIds[j]; break;
                        case 3: entity.UpPurpleItem4 = purpleIds[j]; break;
                        case 4: entity.UpPurpleItem5 = purpleIds[j]; break;
                    }
                }
            }

            await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await metadataDbContext.GachaEvents.Where(g => g.Locale == locale).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
                await metadataDbContext.GachaEvents.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Locale}] gacha_events: {Count} 条", locale, entities.Count);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Locale}] 刷新 gacha_events 失败", locale);
        }
    }

    private async Task RefreshTableAsync<TEntity>(
        string locale,
        string localePath,
        string fileName,
        Type tableType,
        Func<IQueryable<TEntity>> query,
        CancellationToken cancellationToken,
        Func<string, string, List<TEntity>?> converter)
        where TEntity : class
    {
        try
        {
            string filePath = Path.Combine(localePath, fileName);
            if (!File.Exists(filePath))
            {
                logger.LogWarning("[{Locale}] 文件不存在: {File}", locale, fileName);
                return;
            }

            string json = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);
            var entities = converter(json, locale);

            if (entities != null && entities.Count > 0)
            {
                await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await query().ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
                    await metadataDbContext.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                    await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                    logger.LogInformation("[{Locale}] {Table}: {Count} 条", locale, TableName(fileName), entities.Count);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Locale}] 刷新 {Table} 失败", locale, TableName(fileName));
        }
    }

    private string TableName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    private async Task RefreshKnownItemsAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("开始刷新已知物品元数据");

            await EnsureGitRepo(cancellationToken).ConfigureAwait(false);

            string chsPath = Path.Combine(RepoDirectory, "Genshin", "CHS");

            var knownItems = new List<KnownItem>();

            // Read Material.json
            string materialPath = Path.Combine(chsPath, "Material.json");
            if (File.Exists(materialPath))
            {
                string json = await File.ReadAllTextAsync(materialPath, cancellationToken).ConfigureAwait(false);
                var materials = JsonSerializer.Deserialize<List<Material>>(json);
                if (materials != null)
                {
                    foreach (var m in materials)
                    {
                        AddKnownItem(knownItems, m.Id, m.RankLevel);
                    }
                }
            }

            // Read DisplayItem.json
            string displayItemPath = Path.Combine(chsPath, "DisplayItem.json");
            if (File.Exists(displayItemPath))
            {
                string json = await File.ReadAllTextAsync(displayItemPath, cancellationToken).ConfigureAwait(false);
                var displayItems = JsonSerializer.Deserialize<List<DisplayItem>>(json);
                if (displayItems != null)
                {
                    foreach (var d in displayItems)
                    {
                        AddKnownItem(knownItems, d.Id, d.RankLevel);
                    }
                }
            }

            // Read Weapon.json
            string weaponPath = Path.Combine(chsPath, "Weapon.json");
            if (File.Exists(weaponPath))
            {
                string json = await File.ReadAllTextAsync(weaponPath, cancellationToken).ConfigureAwait(false);
                var weapons = JsonSerializer.Deserialize<List<Weapon>>(json);
                if (weapons != null)
                {
                    foreach (var w in weapons)
                    {
                        AddKnownItem(knownItems, w.Id, w.RankLevel);
                    }
                }
            }

            // Read Avatar/*.json scattered
            string avatarDir = Path.Combine(chsPath, "Avatar");
            if (Directory.Exists(avatarDir))
            {
                foreach (string file in Directory.GetFiles(avatarDir, "*.json"))
                {
                    string avatarJson = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
                    var avatar = JsonSerializer.Deserialize<Avatar>(avatarJson);
                    if (avatar != null)
                    {
                        AddKnownItem(knownItems, avatar.Id, avatar.Quality);
                    }
                }
            }

            // Bulk replace
            await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await metadataDbContext.KnownItems.ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
                await metadataDbContext.KnownItems.AddRangeAsync(knownItems, cancellationToken).ConfigureAwait(false);
                await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("成功更新已知物品元数据，共 {Count} 条记录", knownItems.Count);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新已知物品元数据时发生错误");
        }
    }

    private void AddKnownItem(List<KnownItem> knownItems, uint id, uint quality)
    {
        if (!knownItems.Any(x => x.Id == id))
        {
            knownItems.Add(new KnownItem { Id = id, Quality = quality });
        }
    }

    private async Task EnsureGitRepo(CancellationToken cancellationToken = default)
    {
        GitRepository? gitRepo = await appDbContext.GitRepositories
            .FirstOrDefaultAsync(r => r.Name == "Snap.Metadata", cancellationToken)
            .ConfigureAwait(false);

        string directory = RepoDirectory;

        if (gitRepo == null)
        {
            logger.LogError("未找到Snap.Metadata仓库配置");
            return;
        }

        FetchOptions fetchOptions = new()
        {
            Depth = 1,
            Prune = true,
            TagFetchMode = TagFetchMode.None,
            ProxyOptions = { ProxyType = ProxyType.Auto },
            CertificateCheck = static (cert, valid, host) => true,
        };

        if (!Repository.IsValid(directory))
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            Repository.AdvancedClone(gitRepo.HttpsUrl, directory, new(fetchOptions)
            {
                Checkout = true,
            });
        }
        else
        {
            using Repository repo = new(directory);
            Configuration config = repo.Config;
            config.Set("core.longpaths", true);
            config.Set("safe.directory", true);
            if (string.IsNullOrEmpty(fetchOptions.ProxyOptions.Url))
            {
                config.Unset("http.proxy");
                config.Unset("https.proxy");
            }
            else
            {
                config.Set("http.proxy", fetchOptions.ProxyOptions.Url);
                config.Set("https.proxy", fetchOptions.ProxyOptions.Url);
            }

            repo.Network.Remotes.Update("origin", remote => remote.Url = gitRepo.HttpsUrl);
            repo.RemoveUntrackedFiles();
            fetchOptions.UpdateFetchHead = false;
            Commands.Fetch(repo, repo.Head.RemoteName, Array.Empty<string>(), fetchOptions, default);

            File.WriteAllText(Path.Combine(directory, ".git//shallow"), string.Join("", repo.Branches.Where(static branch => branch.IsRemote).Select(static branch => $"{branch.Tip.Sha}\n")));

            Branch remoteBranch = repo.Branches["origin/main"];
            Branch localBranch = repo.Branches["main"] ?? repo.CreateBranch("main", remoteBranch.Tip);
            repo.Branches.Update(localBranch, b => b.TrackedBranch = remoteBranch.CanonicalName);
            repo.Reset(ResetMode.Hard, remoteBranch.Tip);
            repo.RemoveUntrackedFiles();
        }
    }
}
