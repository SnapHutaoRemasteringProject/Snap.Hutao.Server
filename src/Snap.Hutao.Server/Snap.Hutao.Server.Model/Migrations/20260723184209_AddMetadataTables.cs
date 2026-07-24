using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Server.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddMetadataTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // 清理 EnsureCreated() 遗留的旧表
            migrationBuilder.Sql("DROP TABLE IF EXISTS `gacha_events`");
            migrationBuilder.Sql("DROP TABLE IF EXISTS `known_items`");

            migrationBuilder.CreateTable(
                name: "gacha_events",
                columns: table => new
                {
                    Version = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Locale = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<uint>(type: "int unsigned", nullable: false),
                    From = table.Column<DateTime>(type: "datetime", nullable: false),
                    To = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpOrangeItem1 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem2 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem3 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem4 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem5 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem6 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem7 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem8 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem9 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem10 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem11 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem12 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem13 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem14 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem15 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem16 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpOrangeItem17 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpPurpleItem1 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpPurpleItem2 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpPurpleItem3 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpPurpleItem4 = table.Column<uint>(type: "int unsigned", nullable: false),
                    UpPurpleItem5 = table.Column<uint>(type: "int unsigned", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gacha_events", x => new { x.Version, x.Name, x.Locale, x.Order });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "known_items",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quality = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_known_items", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_achievement_goals",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<uint>(type: "int unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FinishReward = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_achievement_goals", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_achievements",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Goal = table.Column<uint>(type: "int unsigned", nullable: false),
                    Order = table.Column<uint>(type: "int unsigned", nullable: false),
                    Title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Progress = table.Column<uint>(type: "int unsigned", nullable: false),
                    Version = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDailyQuest = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreviousId = table.Column<uint>(type: "int unsigned", nullable: false),
                    FinishReward = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_achievements", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_avatars",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quality = table.Column<uint>(type: "int unsigned", nullable: false),
                    Weapon = table.Column<uint>(type: "int unsigned", nullable: false),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SideIcon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<uint>(type: "int unsigned", nullable: false),
                    Body = table.Column<uint>(type: "int unsigned", nullable: false),
                    BaseValue = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrowCurves = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SkillDepot = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FetterInfo = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Costumes = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_avatars", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_beyond_items",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<uint>(type: "int unsigned", nullable: false),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_beyond_items", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_chapters",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageTitle = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CityId = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_chapters", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_display_items",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeDescription = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    ItemType = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_display_items", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_furniture",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comfort = table.Column<uint>(type: "int unsigned", nullable: false),
                    Cost = table.Column<uint>(type: "int unsigned", nullable: false),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    Types = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceTexts = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_furniture", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_furniture_makes",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Experience = table.Column<uint>(type: "int unsigned", nullable: false),
                    Materials = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_furniture_makes", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_furniture_suites",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Types = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Units = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_furniture_suites", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_furniture_types",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<uint>(type: "int unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name2 = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TabIcon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_furniture_types", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_hyper_link_names",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_hyper_link_names", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_materials",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeDescription = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    ItemType = table.Column<uint>(type: "int unsigned", nullable: false),
                    MaterialType = table.Column<uint>(type: "int unsigned", nullable: false),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_materials", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_monsters",
                columns: table => new
                {
                    DescribeId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<uint>(type: "int unsigned", nullable: false),
                    Arkhe = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_monsters", x => new { x.DescribeId, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_name_cards",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    Pictures = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_name_cards", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_profile_pictures",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnlockType = table.Column<uint>(type: "int unsigned", nullable: false),
                    UnlockParameter = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_profile_pictures", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_reliquaries",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SetId = table.Column<uint>(type: "int unsigned", nullable: false),
                    EquipType = table.Column<uint>(type: "int unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    Ids = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_reliquaries", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_reliquary_sets",
                columns: table => new
                {
                    SetId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EquipAffixIds = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NeedNumber = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descriptions = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_reliquary_sets", x => new { x.SetId, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_role_combat_schedules",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Begin = table.Column<DateTime>(type: "datetime", nullable: false),
                    End = table.Column<DateTime>(type: "datetime", nullable: false),
                    Elements = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecialAvatars = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InitialAvatars = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_role_combat_schedules", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_tower_schedules",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Open = table.Column<DateTime>(type: "datetime", nullable: false),
                    Close = table.Column<DateTime>(type: "datetime", nullable: false),
                    BuffName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FloorIds = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descriptions = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_tower_schedules", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_weapons",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    WeaponType = table.Column<uint>(type: "int unsigned", nullable: false),
                    Icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AwakenIcon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrowCurves = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Affix = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CultivationItems = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_weapons", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gacha_events");

            migrationBuilder.DropTable(
                name: "known_items");

            migrationBuilder.DropTable(
                name: "metadata_achievement_goals");

            migrationBuilder.DropTable(
                name: "metadata_achievements");

            migrationBuilder.DropTable(
                name: "metadata_avatars");

            migrationBuilder.DropTable(
                name: "metadata_beyond_items");

            migrationBuilder.DropTable(
                name: "metadata_chapters");

            migrationBuilder.DropTable(
                name: "metadata_display_items");

            migrationBuilder.DropTable(
                name: "metadata_furniture");

            migrationBuilder.DropTable(
                name: "metadata_furniture_makes");

            migrationBuilder.DropTable(
                name: "metadata_furniture_suites");

            migrationBuilder.DropTable(
                name: "metadata_furniture_types");

            migrationBuilder.DropTable(
                name: "metadata_hyper_link_names");

            migrationBuilder.DropTable(
                name: "metadata_materials");

            migrationBuilder.DropTable(
                name: "metadata_monsters");

            migrationBuilder.DropTable(
                name: "metadata_name_cards");

            migrationBuilder.DropTable(
                name: "metadata_profile_pictures");

            migrationBuilder.DropTable(
                name: "metadata_reliquaries");

            migrationBuilder.DropTable(
                name: "metadata_reliquary_sets");

            migrationBuilder.DropTable(
                name: "metadata_role_combat_schedules");

            migrationBuilder.DropTable(
                name: "metadata_tower_schedules");

            migrationBuilder.DropTable(
                name: "metadata_weapons");
        }
    }
}
