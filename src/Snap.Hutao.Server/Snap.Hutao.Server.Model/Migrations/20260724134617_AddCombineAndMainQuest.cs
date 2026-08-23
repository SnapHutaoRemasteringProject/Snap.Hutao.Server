// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Server.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddCombineAndMainQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChapterIcon",
                table: "metadata_chapters",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_combines",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<uint>(type: "int unsigned", nullable: false),
                    SubType = table.Column<uint>(type: "int unsigned", nullable: false),
                    RecipeType = table.Column<uint>(type: "int unsigned", nullable: false),
                    Cost = table.Column<uint>(type: "int unsigned", nullable: false),
                    Result = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Materials = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EffectDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_combines", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metadata_main_quests",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    Locale = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<uint>(type: "int unsigned", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnlockDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChapterId = table.Column<uint>(type: "int unsigned", nullable: false),
                    SortWeight = table.Column<uint>(type: "int unsigned", nullable: false),
                    RecommendLevel = table.Column<uint>(type: "int unsigned", nullable: false),
                    ActivityId = table.Column<uint>(type: "int unsigned", nullable: false),
                    MainQuestTag = table.Column<uint>(type: "int unsigned", nullable: false),
                    ShowType = table.Column<uint>(type: "int unsigned", nullable: false),
                    Repeatable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Series = table.Column<uint>(type: "int unsigned", nullable: false),
                    TaskId = table.Column<uint>(type: "int unsigned", nullable: false),
                    RewardList = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata_main_quests", x => new { x.Id, x.Locale });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "metadata_combines");

            migrationBuilder.DropTable(
                name: "metadata_main_quests");

            migrationBuilder.DropColumn(
                name: "ChapterIcon",
                table: "metadata_chapters");
        }
    }
}
