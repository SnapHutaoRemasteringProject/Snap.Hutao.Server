// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Metadata.Service;
using Snap.Hutao.Server.Model.Context;

namespace Snap.Hutao.Server.Metadata.Controller;

[Route("")]
[ApiExplorerSettings(GroupName = "Metadata")]
public sealed class MetadataController : ControllerBase
{
    private readonly MetadataDbContext metadataDbContext;
    private readonly MetadataRefreshService refreshService;

    public MetadataController(MetadataDbContext metadataDbContext, MetadataRefreshService refreshService)
    {
        this.metadataDbContext = metadataDbContext;
        this.refreshService = refreshService;
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        await refreshService.RefreshAllAsync().ConfigureAwait(false);
        return Ok(new { message = "元数据刷新完成" });
    }

    [HttpGet("avatars")]
    public async Task<IActionResult> GetAvatars([FromQuery] string? locale)
    {
        var query = metadataDbContext.Avatars.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(a => a.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("avatars/{id}")]
    public async Task<IActionResult> GetAvatar(uint id, [FromQuery] string? locale)
    {
        var query = metadataDbContext.Avatars.Where(a => a.Id == id);
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(a => a.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("weapons")]
    public async Task<IActionResult> GetWeapons([FromQuery] string? locale)
    {
        var query = metadataDbContext.Weapons.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(w => w.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("weapons/{id}")]
    public async Task<IActionResult> GetWeapon(uint id, [FromQuery] string? locale)
    {
        var query = metadataDbContext.Weapons.Where(w => w.Id == id);
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(w => w.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("achievements")]
    public async Task<IActionResult> GetAchievements([FromQuery] string? locale)
    {
        var query = metadataDbContext.Achievements.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(a => a.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("achievement-goals")]
    public async Task<IActionResult> GetAchievementGoals([FromQuery] string? locale)
    {
        var query = metadataDbContext.AchievementGoals.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(a => a.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("chapters")]
    public async Task<IActionResult> GetChapters([FromQuery] string? locale)
    {
        var query = metadataDbContext.Chapters.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(c => c.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("display-items")]
    public async Task<IActionResult> GetDisplayItems([FromQuery] string? locale)
    {
        var query = metadataDbContext.DisplayItems.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(d => d.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("materials")]
    public async Task<IActionResult> GetMaterials([FromQuery] string? locale)
    {
        var query = metadataDbContext.Materials.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(m => m.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("monsters")]
    public async Task<IActionResult> GetMonsters([FromQuery] string? locale)
    {
        var query = metadataDbContext.Monsters.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(m => m.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("name-cards")]
    public async Task<IActionResult> GetNameCards([FromQuery] string? locale)
    {
        var query = metadataDbContext.NameCards.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(n => n.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("profile-pictures")]
    public async Task<IActionResult> GetProfilePictures([FromQuery] string? locale)
    {
        var query = metadataDbContext.ProfilePictures.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(p => p.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("reliquaries")]
    public async Task<IActionResult> GetReliquaries([FromQuery] string? locale)
    {
        var query = metadataDbContext.Reliquaries.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(r => r.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("reliquary-sets")]
    public async Task<IActionResult> GetReliquarySets([FromQuery] string? locale)
    {
        var query = metadataDbContext.ReliquarySets.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(r => r.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("tower-schedules")]
    public async Task<IActionResult> GetTowerSchedules([FromQuery] string? locale)
    {
        var query = metadataDbContext.TowerSchedules.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(t => t.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("furniture")]
    public async Task<IActionResult> GetFurniture([FromQuery] string? locale)
    {
        var query = metadataDbContext.Furniture.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(f => f.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("furniture-makes")]
    public async Task<IActionResult> GetFurnitureMakes([FromQuery] string? locale)
    {
        var query = metadataDbContext.FurnitureMakes.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(f => f.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("furniture-suites")]
    public async Task<IActionResult> GetFurnitureSuites([FromQuery] string? locale)
    {
        var query = metadataDbContext.FurnitureSuites.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(f => f.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("furniture-types")]
    public async Task<IActionResult> GetFurnitureTypes([FromQuery] string? locale)
    {
        var query = metadataDbContext.FurnitureTypes.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(f => f.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("role-combat-schedules")]
    public async Task<IActionResult> GetRoleCombatSchedules([FromQuery] string? locale)
    {
        var query = metadataDbContext.RoleCombatSchedules.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(r => r.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("beyond-items")]
    public async Task<IActionResult> GetBeyondItems([FromQuery] string? locale)
    {
        var query = metadataDbContext.BeyondItems.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(b => b.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("hyper-link-names")]
    public async Task<IActionResult> GetHyperLinkNames([FromQuery] string? locale)
    {
        var query = metadataDbContext.HyperLinkNames.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(h => h.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("gacha-events")]
    public async Task<IActionResult> GetGachaEvents([FromQuery] string? locale)
    {
        var query = metadataDbContext.GachaEvents.AsQueryable();
        if (!string.IsNullOrEmpty(locale))
        {
            query = query.Where(g => g.Locale == locale);
        }

        var items = await query.ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] string? locale, [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new { message = "搜索关键词不能为空" });
        }

        var result = new Dictionary<string, object>();

        var loc = locale ?? "CHS";

        result["avatars"] = await metadataDbContext.Avatars
            .Where(a => a.Locale == loc && a.Name != null && a.Name.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        result["weapons"] = await metadataDbContext.Weapons
            .Where(w => w.Locale == loc && w.Name != null && w.Name.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        result["materials"] = await metadataDbContext.Materials
            .Where(m => m.Locale == loc && m.Name != null && m.Name.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        result["display_items"] = await metadataDbContext.DisplayItems
            .Where(d => d.Locale == loc && d.Name != null && d.Name.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        result["monsters"] = await metadataDbContext.Monsters
            .Where(m => m.Locale == loc && m.Name != null && m.Name.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        result["achievements"] = await metadataDbContext.Achievements
            .Where(a => a.Locale == loc && a.Title != null && a.Title.Contains(q))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);

        return Ok(result);
    }

    [HttpGet("avatar/search")]
    public async Task<IActionResult> SearchAvatars([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "搜索关键词不能为空" });
        }

        var loc = locale ?? "CHS";
        var items = await metadataDbContext.Avatars
            .Where(a => a.Locale == loc && a.Name != null && a.Name.Contains(name))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("weapon/search")]
    public async Task<IActionResult> SearchWeapons([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "搜索关键词不能为空" });
        }

        var loc = locale ?? "CHS";
        var items = await metadataDbContext.Weapons
            .Where(w => w.Locale == loc && w.Name != null && w.Name.Contains(name))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }

    [HttpGet("material/search")]
    public async Task<IActionResult> SearchMaterials([FromQuery] string name, [FromQuery] string? locale, [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "搜索关键词不能为空" });
        }

        var loc = locale ?? "CHS";
        var items = await metadataDbContext.Materials
            .Where(m => m.Locale == loc && m.Name != null && m.Name.Contains(name))
            .Take(limit)
            .ToListAsync().ConfigureAwait(false);
        return Ok(items);
    }
}
