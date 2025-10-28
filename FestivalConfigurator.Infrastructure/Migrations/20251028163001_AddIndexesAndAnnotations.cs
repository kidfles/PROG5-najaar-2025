using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestivalConfigurator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesAndAnnotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemType_Name",
                table: "Items",
                columns: new[] { "ItemType", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Festivals_Name",
                table: "Festivals",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_ItemType_Name",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Festivals_Name",
                table: "Festivals");
        }
    }
}
