// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using LibGit2Sharp;
using Newtonsoft.Json.Linq;
using Snap.Hutao.Server.Extension;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Metadata;

namespace Snap.Hutao.Server.Service.Metadata;

// Scoped
public sealed class MetadataRefreshService
{
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

    public async Task RefreshGachaEventsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("开始刷新祈愿活动元数据");

            await EnsureGitRepo(cancellationToken);

            string path = "Snap.Metadata";

            // 1. 读取GachaEvent.json文件
            string jsonFilePath = Path.Combine(path, "Genshin", "CHS", "GachaEvent.json");
            if (!File.Exists(jsonFilePath))
            {
                logger.LogError("未找到GachaEvent.json文件: {Path}", jsonFilePath);
                return;
            }

            string jsonContent = await File.ReadAllTextAsync(jsonFilePath, cancellationToken).ConfigureAwait(false);
            var gachaEvents = JsonSerializer.Deserialize<List<GachaEventJson>>(jsonContent);

            if (gachaEvents == null || gachaEvents.Count == 0)
            {
                logger.LogWarning("GachaEvent.json文件为空或解析失败");
                return;
            }

            // 2. 转换为实体并保存到数据库
            var entities = gachaEvents.Select(ConvertToEntity).ToList();

            // 使用事务确保数据一致性
            await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                // 清空现有数据
                await metadataDbContext.GachaEvents.ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);

                // 插入新数据
                await metadataDbContext.GachaEvents.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("成功更新祈愿活动元数据，共 {Count} 条记录", entities.Count);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                logger.LogError(ex, "保存祈愿活动数据到数据库时发生错误");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新祈愿活动元数据时发生错误");
            throw;
        }
    }

    private static GachaEventInfo ConvertToEntity(GachaEventJson json)
    {
        var entity = new GachaEventInfo
        {
            Version = json.Version,
            Name = json.Name,
            Locale = "CHS",
            Order = (uint)json.Order,
            From = DateTime.Parse(json.From),
            To = DateTime.Parse(json.To),
            Type = (GachaConfigType)json.Type,
        };

        for (int i = 0; i < Math.Min(json.UpOrangeList.Count, 16); i++)
        {
            switch (i)
            {
                case 0: entity.UpOrangeItem1 = (uint)json.UpOrangeList[i]; break;
                case 1: entity.UpOrangeItem2 = (uint)json.UpOrangeList[i]; break;
                case 2: entity.UpOrangeItem3 = (uint)json.UpOrangeList[i]; break;
                case 3: entity.UpOrangeItem4 = (uint)json.UpOrangeList[i]; break;
                case 4: entity.UpOrangeItem5 = (uint)json.UpOrangeList[i]; break;
                case 5: entity.UpOrangeItem6 = (uint)json.UpOrangeList[i]; break;
                case 6: entity.UpOrangeItem7 = (uint)json.UpOrangeList[i]; break;
                case 7: entity.UpOrangeItem8 = (uint)json.UpOrangeList[i]; break;
                case 8: entity.UpOrangeItem9 = (uint)json.UpOrangeList[i]; break;
                case 9: entity.UpOrangeItem10 = (uint)json.UpOrangeList[i]; break;
                case 10: entity.UpOrangeItem11 = (uint)json.UpOrangeList[i]; break;
                case 11: entity.UpOrangeItem12 = (uint)json.UpOrangeList[i]; break;
                case 12: entity.UpOrangeItem13 = (uint)json.UpOrangeList[i]; break;
                case 13: entity.UpOrangeItem14 = (uint)json.UpOrangeList[i]; break;
                case 14: entity.UpOrangeItem15 = (uint)json.UpOrangeList[i]; break;
                case 15: entity.UpOrangeItem16 = (uint)json.UpOrangeList[i]; break;
            }
        }

        for (int i = 0; i < Math.Min(json.UpPurpleList.Count, 5); i++)
        {
            switch (i)
            {
                case 0: entity.UpPurpleItem1 = (uint)json.UpPurpleList[i]; break;
                case 1: entity.UpPurpleItem2 = (uint)json.UpPurpleList[i]; break;
                case 2: entity.UpPurpleItem3 = (uint)json.UpPurpleList[i]; break;
                case 3: entity.UpPurpleItem4 = (uint)json.UpPurpleList[i]; break;
                case 4: entity.UpPurpleItem5 = (uint)json.UpPurpleList[i]; break;
            }
        }

        return entity;
    }

    public async Task RefreshKnownItemsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("开始刷新已知物品元数据");

            await EnsureGitRepo(cancellationToken);

            string path = "Snap.Metadata";

            // 1. 读取Material.json文件
            string materialJsonPath = Path.Combine(path, "Genshin", "CHS", "Material.json");
            if (!File.Exists(materialJsonPath))
            {
                logger.LogError("未找到Material.json文件: {Path}", materialJsonPath);
                return;
            }

            string materialJsonContent = await File.ReadAllTextAsync(materialJsonPath, cancellationToken).ConfigureAwait(false);
            var materials = (JArray)JContainer.Parse(materialJsonContent);

            // 2. 读取DisplayItem.json文件
            string displayItemJsonPath = Path.Combine(path, "Genshin", "CHS", "DisplayItem.json");
            if (!File.Exists(displayItemJsonPath))
            {
                logger.LogError("未找到DisplayItem.json文件: {Path}", displayItemJsonPath);
                return;
            }

            string displayItemJsonContent = await File.ReadAllTextAsync(displayItemJsonPath, cancellationToken).ConfigureAwait(false);
            var displayItems = (JArray)JContainer.Parse(displayItemJsonContent);

            // 3. 读取Weapon.json文件
            string weaponJsonPath = Path.Combine(path, "Genshin", "CHS", "Weapon.json");
            if (!File.Exists(weaponJsonPath))
            {
                logger.LogError("Weapon.json文件: {Path}", weaponJsonPath);
                return;
            }

            string weaponJsonContent = await File.ReadAllTextAsync(weaponJsonPath, cancellationToken).ConfigureAwait(false);
            var weapons = (JArray)JContainer.Parse(weaponJsonContent);

            if ((materials == null || materials.Count == 0) && (displayItems == null || displayItems.Count == 0) && (weapons == null || weapons.Count == 0))
            {
                logger.LogWarning("Material.json和DisplayItem.json和Weapon.json文件都为空或解析失败");
                return;
            }

            var knownItems = new List<KnownItem>();

            AddKnownItems(knownItems, materials);
            AddKnownItems(knownItems, displayItems);
            AddKnownItems(knownItems, weapons);

            // 4. 保存到数据库
            await using var transaction = await metadataDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                // 清空现有数据
                await metadataDbContext.KnownItems.ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);

                // 插入新数据
                await metadataDbContext.KnownItems.AddRangeAsync(knownItems, cancellationToken).ConfigureAwait(false);
                await metadataDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("成功更新已知物品元数据，共 {Count} 条记录", knownItems.Count);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                logger.LogError(ex, "保存已知物品数据到数据库时发生错误");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新已知物品元数据时发生错误");
            throw;
        }
    }

    private static void AddKnownItems(List<KnownItem> knownItems, JArray? array)
    {
        if (array != null)
        {
            foreach (var item in array)
            {
                if (!knownItems.Any(x => x.Id == (uint)item["Id"]!))
                {
                    knownItems.Add(new KnownItem
                    {
                        Id = (uint)item["Id"]!,
                        Quality = (uint)item["RankLevel"]!,
                    });
                }
            }
        }
    }

    private async Task EnsureGitRepo(CancellationToken cancellationToken = default)
    {
        GitRepository? gitRepo = await appDbContext.GitRepositories
            .FirstOrDefaultAsync(r => r.Name == "Snap.Metadata", cancellationToken)
            .ConfigureAwait(false);

        string directory = "Snap.Metadata";

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
            ProxyOptions =
    {
        ProxyType = ProxyType.Auto,
    },
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
            // We need to ensure local repo is up to date
            using (Repository repo = new(directory))
            {
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

                // Manually patch .git/shallow file
                File.WriteAllText(Path.Combine(directory, ".git//shallow"), string.Join("", repo.Branches.Where(static branch => branch.IsRemote).Select(static branch => $"{branch.Tip.Sha}\n")));

                Branch remoteBranch = repo.Branches["origin/main"];
                Branch localBranch = repo.Branches["main"] ?? repo.CreateBranch("main", remoteBranch.Tip);
                repo.Branches.Update(localBranch, b => b.TrackedBranch = remoteBranch.CanonicalName);
                repo.Reset(ResetMode.Hard, remoteBranch.Tip);
                repo.RemoveUntrackedFiles();
            }
        }
    }

    private sealed class GachaEventJson
    {
        public string Name { get; set; } = default!;

        public string Version { get; set; } = default!;

        public int Order { get; set; }

        public string Banner { get; set; } = default!;

        public string Banner2 { get; set; } = default!;

        public string From { get; set; } = default!;

        public string To { get; set; } = default!;

        public int Type { get; set; }

        public List<int> UpOrangeList { get; set; } = default!;

        public List<int> UpPurpleList { get; set; } = default!;
    }
}
