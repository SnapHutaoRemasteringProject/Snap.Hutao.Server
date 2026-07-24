// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Metadata;

namespace Snap.Hutao.Server.Model.Context;

public sealed class MetadataDbContext : DbContext
{
    public MetadataDbContext(DbContextOptions<MetadataDbContext> options)
        : base(options)
    {
    }

    public DbSet<GachaEventInfo> GachaEvents { get; set; }

    public DbSet<KnownItem> KnownItems { get; set; }

    public DbSet<MetadataAchievement> Achievements { get; set; }

    public DbSet<MetadataAchievementGoal> AchievementGoals { get; set; }

    public DbSet<MetadataAvatar> Avatars { get; set; }

    public DbSet<MetadataBeyondItem> BeyondItems { get; set; }

    public DbSet<MetadataChapter> Chapters { get; set; }

    public DbSet<MetadataDisplayItem> DisplayItems { get; set; }

    public DbSet<MetadataFurniture> Furniture { get; set; }

    public DbSet<MetadataFurnitureMake> FurnitureMakes { get; set; }

    public DbSet<MetadataFurnitureSuite> FurnitureSuites { get; set; }

    public DbSet<MetadataFurnitureType> FurnitureTypes { get; set; }

    public DbSet<MetadataHyperLinkName> HyperLinkNames { get; set; }

    public DbSet<MetadataMaterial> Materials { get; set; }

    public DbSet<MetadataMonster> Monsters { get; set; }

    public DbSet<MetadataNameCard> NameCards { get; set; }

    public DbSet<MetadataProfilePicture> ProfilePictures { get; set; }

    public DbSet<MetadataReliquary> Reliquaries { get; set; }

    public DbSet<MetadataReliquarySet> ReliquarySets { get; set; }

    public DbSet<MetadataRoleCombatSchedule> RoleCombatSchedules { get; set; }

    public DbSet<MetadataTowerSchedule> TowerSchedules { get; set; }

    public DbSet<MetadataWeapon> Weapons { get; set; }
}