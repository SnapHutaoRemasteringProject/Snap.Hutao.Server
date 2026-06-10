using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                table: "hutao_package_informations",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "MSIX")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "hutao_package_informations");
        }
    }
}
