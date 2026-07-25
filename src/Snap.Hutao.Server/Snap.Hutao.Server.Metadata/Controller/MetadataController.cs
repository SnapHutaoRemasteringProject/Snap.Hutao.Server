// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Metadata.Option;
using Snap.Hutao.Server.Metadata.Service;
using Snap.Hutao.Server.Model.Context;

namespace Snap.Hutao.Server.Metadata.Controller;

[Route("")]
[ApiExplorerSettings(GroupName = "Metadata")]
public sealed class MetadataController : ControllerBase
{
    private const string PasswordHeader = "X-Metadata-Key";

    private readonly MetadataDbContext metadataDbContext;
    private readonly MetadataRefreshService refreshService;
    private readonly Option.AppOptions appOptions;

    public MetadataController(MetadataDbContext metadataDbContext, MetadataRefreshService refreshService, Option.AppOptions appOptions)
    {
        this.metadataDbContext = metadataDbContext;
        this.refreshService = refreshService;
        this.appOptions = appOptions;
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAsync([FromHeader(Name = PasswordHeader)] string? password)
    {
        if (string.IsNullOrEmpty(password) || password != appOptions.RefreshPassword)
        {
            return Unauthorized(new { message = "无效的刷新密钥" });
        }

        await refreshService.RefreshAllAsync().ConfigureAwait(false);
        return Ok(new { message = "元数据刷新完成" });
    }

    // ---- Avatar ----
    [HttpGet("avatars")] public Task<IActionResult> GetAvatars([FromQuery] string? locale) => ListById(metadataDbContext.Avatars, locale);
    [HttpGet("avatars/{id}")] public Task<IActionResult> GetAvatar(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Avatars.Where(a => a.Id == id), locale);

    // ---- Weapon ----
    [HttpGet("weapons")] public Task<IActionResult> GetWeapons([FromQuery] string? locale) => ListById(metadataDbContext.Weapons, locale);
    [HttpGet("weapons/{id}")] public Task<IActionResult> GetWeapon(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Weapons.Where(w => w.Id == id), locale);

    // ---- Achievement ----
    [HttpGet("achievements")] public Task<IActionResult> GetAchievements([FromQuery] string? locale) => ListById(metadataDbContext.Achievements, locale);
    [HttpGet("achievements/{id}")] public Task<IActionResult> GetAchievement(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Achievements.Where(a => a.Id == id), locale);

    // ---- AchievementGoal ----
    [HttpGet("achievement-goals")] public Task<IActionResult> GetAchievementGoals([FromQuery] string? locale) => ListById(metadataDbContext.AchievementGoals, locale);
    [HttpGet("achievement-goals/{id}")] public Task<IActionResult> GetAchievementGoal(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.AchievementGoals.Where(a => a.Id == id), locale);

    // ---- Chapter ----
    [HttpGet("chapters")] public Task<IActionResult> GetChapters([FromQuery] string? locale) => ListById(metadataDbContext.Chapters, locale);
    [HttpGet("chapters/{id}")] public Task<IActionResult> GetChapter(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Chapters.Where(c => c.Id == id), locale);

    // ---- Combine ----
    [HttpGet("combines")] public Task<IActionResult> GetCombines([FromQuery] string? locale) => ListById(metadataDbContext.Combines, locale);
    [HttpGet("combines/{id}")] public Task<IActionResult> GetCombine(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Combines.Where(c => c.Id == id), locale);

    // ---- MainQuest ----
    [HttpGet("main-quests")] public Task<IActionResult> GetMainQuests([FromQuery] string? locale) => ListById(metadataDbContext.MainQuests, locale);
    [HttpGet("main-quests/{id}")] public Task<IActionResult> GetMainQuest(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.MainQuests.Where(m => m.Id == id), locale);

    // ---- DisplayItem ----
    [HttpGet("display-items")] public Task<IActionResult> GetDisplayItems([FromQuery] string? locale) => ListById(metadataDbContext.DisplayItems, locale);
    [HttpGet("display-items/{id}")] public Task<IActionResult> GetDisplayItem(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.DisplayItems.Where(d => d.Id == id), locale);

    // ---- Material ----
    [HttpGet("materials")] public Task<IActionResult> GetMaterials([FromQuery] string? locale) => ListById(metadataDbContext.Materials, locale);
    [HttpGet("materials/{id}")] public Task<IActionResult> GetMaterial(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Materials.Where(m => m.Id == id), locale);

    // ---- Monster ----
    [HttpGet("monsters")] public Task<IActionResult> GetMonsters([FromQuery] string? locale) => ListById(metadataDbContext.Monsters, locale);
    [HttpGet("monsters/{id}")] public Task<IActionResult> GetMonster(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Monsters.Where(m => m.Id == id), locale);

    // ---- NameCard ----
    [HttpGet("name-cards")] public Task<IActionResult> GetNameCards([FromQuery] string? locale) => ListById(metadataDbContext.NameCards, locale);
    [HttpGet("name-cards/{id}")] public Task<IActionResult> GetNameCard(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.NameCards.Where(n => n.Id == id), locale);

    // ---- ProfilePicture ----
    [HttpGet("profile-pictures")] public Task<IActionResult> GetProfilePictures([FromQuery] string? locale) => ListById(metadataDbContext.ProfilePictures, locale);
    [HttpGet("profile-pictures/{id}")] public Task<IActionResult> GetProfilePicture(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.ProfilePictures.Where(p => p.Id == id), locale);

    // ---- Reliquary ----
    [HttpGet("reliquaries")] public Task<IActionResult> GetReliquaries([FromQuery] string? locale) => ListById(metadataDbContext.Reliquaries, locale);
    [HttpGet("reliquaries/{id}")] public Task<IActionResult> GetReliquary(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Reliquaries.Where(r => r.Id == id), locale);

    // ---- ReliquarySet ----
    [HttpGet("reliquary-sets")] public Task<IActionResult> GetReliquarySets([FromQuery] string? locale) => ListById(metadataDbContext.ReliquarySets, locale);
    [HttpGet("reliquary-sets/{id}")] public Task<IActionResult> GetReliquarySet(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.ReliquarySets.Where(r => r.SetId == id), locale);

    // ---- TowerSchedule ----
    [HttpGet("tower-schedules")] public Task<IActionResult> GetTowerSchedules([FromQuery] string? locale) => ListById(metadataDbContext.TowerSchedules, locale);
    [HttpGet("tower-schedules/{id}")] public Task<IActionResult> GetTowerSchedule(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.TowerSchedules.Where(t => t.Id == id), locale);

    // ---- Furniture ----
    [HttpGet("furniture")] public Task<IActionResult> GetFurniture([FromQuery] string? locale) => ListById(metadataDbContext.Furniture, locale);
    [HttpGet("furniture/{id}")] public Task<IActionResult> GetFurnitureItem(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.Furniture.Where(f => f.Id == id), locale);

    // ---- FurnitureMake ----
    [HttpGet("furniture-makes")] public Task<IActionResult> GetFurnitureMakes([FromQuery] string? locale) => ListById(metadataDbContext.FurnitureMakes, locale);
    [HttpGet("furniture-makes/{id}")] public Task<IActionResult> GetFurnitureMake(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.FurnitureMakes.Where(f => f.Id == id), locale);

    // ---- FurnitureSuite ----
    [HttpGet("furniture-suites")] public Task<IActionResult> GetFurnitureSuites([FromQuery] string? locale) => ListById(metadataDbContext.FurnitureSuites, locale);
    [HttpGet("furniture-suites/{id}")] public Task<IActionResult> GetFurnitureSuite(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.FurnitureSuites.Where(f => f.Id == id), locale);

    // ---- FurnitureType ----
    [HttpGet("furniture-types")] public Task<IActionResult> GetFurnitureTypes([FromQuery] string? locale) => ListById(metadataDbContext.FurnitureTypes, locale);
    [HttpGet("furniture-types/{id}")] public Task<IActionResult> GetFurnitureType(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.FurnitureTypes.Where(f => f.Id == id), locale);

    // ---- RoleCombatSchedule ----
    [HttpGet("role-combat-schedules")] public Task<IActionResult> GetRoleCombatSchedules([FromQuery] string? locale) => ListById(metadataDbContext.RoleCombatSchedules, locale);
    [HttpGet("role-combat-schedules/{id}")] public Task<IActionResult> GetRoleCombatSchedule(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.RoleCombatSchedules.Where(r => r.Id == id), locale);

    // ---- BeyondItem ----
    [HttpGet("beyond-items")] public Task<IActionResult> GetBeyondItems([FromQuery] string? locale) => ListById(metadataDbContext.BeyondItems, locale);
    [HttpGet("beyond-items/{id}")] public Task<IActionResult> GetBeyondItem(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.BeyondItems.Where(b => b.Id == id), locale);

    // ---- HyperLinkName ----
    [HttpGet("hyper-link-names")] public Task<IActionResult> GetHyperLinkNames([FromQuery] string? locale) => ListById(metadataDbContext.HyperLinkNames, locale);
    [HttpGet("hyper-link-names/{id}")] public Task<IActionResult> GetHyperLinkName(uint id, [FromQuery] string? locale) => ListById(metadataDbContext.HyperLinkNames.Where(h => h.Id == id), locale);

    // ---- GachaEvent ----
    [HttpGet("gacha-events")] public Task<IActionResult> GetGachaEvents([FromQuery] string? locale) => ListById(metadataDbContext.GachaEvents, locale);

    // ---- Search ----
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] string? locale, [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new { message = "搜索关键词不能为空" });
        }

        var result = new Dictionary<string, object>();
        var loc = locale ?? "CHS";

        result["avatars"] = await metadataDbContext.Avatars.Where(a => a.Locale == loc && a.Name != null && a.Name.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);
        result["weapons"] = await metadataDbContext.Weapons.Where(w => w.Locale == loc && w.Name != null && w.Name.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);
        result["materials"] = await metadataDbContext.Materials.Where(m => m.Locale == loc && m.Name != null && m.Name.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);
        result["display_items"] = await metadataDbContext.DisplayItems.Where(d => d.Locale == loc && d.Name != null && d.Name.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);
        result["monsters"] = await metadataDbContext.Monsters.Where(m => m.Locale == loc && m.Name != null && m.Name.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);
        result["achievements"] = await metadataDbContext.Achievements.Where(a => a.Locale == loc && a.Title != null && a.Title.Contains(q)).Take(limit).ToListAsync().ConfigureAwait(false);

        return Ok(result);
    }

    [HttpGet("avatar/search")] public async Task<IActionResult> SearchAvatars([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
        => Ok(await SearchByName(metadataDbContext.Avatars, name, locale, limit));

    [HttpGet("weapon/search")] public async Task<IActionResult> SearchWeapons([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
        => Ok(await SearchByName(metadataDbContext.Weapons, name, locale, limit));

    [HttpGet("material/search")] public async Task<IActionResult> SearchMaterials([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
        => Ok(await SearchByName(metadataDbContext.Materials, name, locale, limit));

    private async Task<IActionResult> ListById<T>(IQueryable<T> source, string? locale)
    {
        if (!string.IsNullOrEmpty(locale))
        {
            source = source.Where(e => EF.Property<string>(e, "Locale") == locale);
        }

        var items = await source.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    private async Task<List<T>> SearchByName<T>(IQueryable<T> source, string name, string? locale, int limit)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return [];
        }

        var loc = locale ?? "CHS";
        return await source
            .Where(e => EF.Property<string>(e, "Locale") == loc && EF.Property<string>(e, "Name") != null && EF.Property<string>(e, "Name")!.Contains(name))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);
    }
}
