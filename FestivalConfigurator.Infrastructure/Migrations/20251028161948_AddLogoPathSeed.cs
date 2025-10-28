using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestivalConfigurator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoPathSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Festivals",
                keyColumn: "Id",
                keyValue: 1,
                column: "Logo",
                value: "/img/logos/lowlands.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Festivals",
                keyColumn: "Id",
                keyValue: 1,
                column: "Logo",
                value: null);
        }
    }
}
