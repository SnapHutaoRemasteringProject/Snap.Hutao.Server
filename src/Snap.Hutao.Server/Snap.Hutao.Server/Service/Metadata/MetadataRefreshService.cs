// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Newtonsoft.Json.Linq;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Metadata;
using System.Diagnostics;
using System.Text.Json;

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

            // 1. 从数据库获取Snap.Metadata仓库信息
            var gitRepo = await appDbContext.GitRepositories
                .FirstOrDefaultAsync(r => r.Name == "Snap.Metadata", cancellationToken)
                .ConfigureAwait(false);

            if (gitRepo == null)
            {
                logger.LogError("未找到Snap.Metadata仓库配置");
                return;
            }

            // 2. 创建临时目录
            string tempPath = Path.Combine(Path.GetTempPath(), $"SnapMetadata_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempPath);

            try
            {
                // 3. 使用系统git命令克隆仓库（避免LibGit2Sharp在Docker环境中的认证问题）
                logger.LogInformation("正在克隆仓库: {Url} 到 {Path}", gitRepo.HttpsUrl, tempPath);

                using var process = new Process();
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = $"clone --depth 1 {gitRepo.HttpsUrl} \"{tempPath}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                string error = await process.StandardError.ReadToEndAsync(cancellationToken);

                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode != 0)
                {
                    logger.LogError("Git克隆失败，退出代码: {ExitCode}, 错误: {Error}", process.ExitCode, error);
                    return;
                }

                logger.LogInformation("Git克隆成功: {Output}", output);

                // 4. 读取GachaEvent.json文件
                string jsonFilePath = Path.Combine(tempPath, "Genshin", "CHS", "GachaEvent.json");
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

                // 5. 转换为实体并保存到数据库
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
            finally
            {
                // 6. 清理临时目录
                try
                {
                    if (Directory.Exists(tempPath))
                    {
                        Directory.Delete(tempPath, true);
                        logger.LogInformation("已清理临时目录: {Path}", tempPath);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "清理临时目录时发生错误: {Path}", tempPath);
                }
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

            // 1. 从数据库获取Snap.Metadata仓库信息
            var gitRepo = await appDbContext.GitRepositories
                .FirstOrDefaultAsync(r => r.Name == "Snap.Metadata", cancellationToken)
                .ConfigureAwait(false);

            if (gitRepo == null)
            {
                logger.LogError("未找到Snap.Metadata仓库配置");
                return;
            }

            // 2. 创建临时目录
            string tempPath = Path.Combine(Path.GetTempPath(), $"SnapMetadata_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempPath);

            try
            {
                // 3. 使用系统git命令克隆仓库
                logger.LogInformation("正在克隆仓库: {Url} 到 {Path}", gitRepo.HttpsUrl, tempPath);

                using var process = new Process();
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = $"clone --depth 1 {gitRepo.HttpsUrl} \"{tempPath}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                string error = await process.StandardError.ReadToEndAsync(cancellationToken);

                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode != 0)
                {
                    logger.LogError("Git克隆失败，退出代码: {ExitCode}, 错误: {Error}", process.ExitCode, error);
                    return;
                }

                logger.LogInformation("Git克隆成功: {Output}", output);

                // 4. 读取Material.json文件
                string materialJsonPath = Path.Combine(tempPath, "Genshin", "CHS", "Material.json");
                if (!File.Exists(materialJsonPath))
                {
                    logger.LogError("未找到Material.json文件: {Path}", materialJsonPath);
                    return;
                }

                string materialJsonContent = await File.ReadAllTextAsync(materialJsonPath, cancellationToken).ConfigureAwait(false);
                var materials = (JArray)JContainer.Parse(materialJsonContent);

                // 5. 读取DisplayItem.json文件
                string displayItemJsonPath = Path.Combine(tempPath, "Genshin", "CHS", "DisplayItem.json");
                if (!File.Exists(displayItemJsonPath))
                {
                    logger.LogError("未找到DisplayItem.json文件: {Path}", displayItemJsonPath);
                    return;
                }

                string displayItemJsonContent = await File.ReadAllTextAsync(displayItemJsonPath, cancellationToken).ConfigureAwait(false);
                var displayItems = (JArray)JContainer.Parse(displayItemJsonContent);

                if ((materials == null || materials.Count == 0) && (displayItems == null || displayItems.Count == 0))
                {
                    logger.LogWarning("Material.json和DisplayItem.json文件都为空或解析失败");
                    return;
                }

                var knownItems = new List<KnownItem>();

                if (materials != null)
                {
                    foreach (var material in materials)
                    {
                        knownItems.Add(new KnownItem
                        {
                            Id = (uint)material["Id"]!,
                            Quality = (uint)material["RankLevel"]!,
                        });
                    }
                }

                if (displayItems != null)
                {
                    foreach (var displayItem in displayItems)
                    {
                        if (!knownItems.Any(x => x.Id == (uint)displayItem["Id"]!))
                        {
                            knownItems.Add(new KnownItem
                            {
                                Id = (uint)displayItem["Id"]!,
                                Quality = (uint)displayItem["RankLevel"]!,
                            });
                        }
                    }
                }

                // 7. 保存到数据库
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
            finally
            {
                // 8. 清理临时目录
                try
                {
                    if (Directory.Exists(tempPath))
                    {
                        Directory.Delete(tempPath, true);
                        logger.LogInformation("已清理临时目录: {Path}", tempPath);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "清理临时目录时发生错误: {Path}", tempPath);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新已知物品元数据时发生错误");
            throw;
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
