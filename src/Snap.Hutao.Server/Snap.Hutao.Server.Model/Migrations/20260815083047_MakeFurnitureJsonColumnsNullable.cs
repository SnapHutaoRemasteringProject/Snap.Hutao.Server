using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Server.Model.Migrations
{
    /// <inheritdoc />
    public partial class MakeFurnitureJsonColumnsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Units",
                table: "metadata_furniture_suites",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Types",
                table: "metadata_furniture_suites",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Types",
                table: "metadata_furniture",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SourceTexts",
                table: "metadata_furniture",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "metadata_furniture_suites",
                keyColumn: "Units",
                keyValue: null,
                column: "Units",
                value: "null");

            migrationBuilder.AlterColumn<string>(
                name: "Units",
                table: "metadata_furniture_suites",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "metadata_furniture_suites",
                keyColumn: "Types",
                keyValue: null,
                column: "Types",
                value: "null");

            migrationBuilder.AlterColumn<string>(
                name: "Types",
                table: "metadata_furniture_suites",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "metadata_furniture",
                keyColumn: "Types",
                keyValue: null,
                column: "Types",
                value: "null");

            migrationBuilder.AlterColumn<string>(
                name: "Types",
                table: "metadata_furniture",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "metadata_furniture",
                keyColumn: "SourceTexts",
                keyValue: null,
                column: "SourceTexts",
                value: "null");

            migrationBuilder.AlterColumn<string>(
                name: "SourceTexts",
                table: "metadata_furniture",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
