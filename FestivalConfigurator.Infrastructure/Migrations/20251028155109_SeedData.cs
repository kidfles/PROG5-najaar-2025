using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FestivalConfigurator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Festivals",
                columns: new[] { "Id", "BasicPrice", "Description", "EndDate", "Logo", "Name", "Place", "StartDate" },
                values: new object[] { 1, 199.00m, "Three-day music festival.", new DateOnly(2025, 8, 24), null, "Lowlands", "Biddinghuizen", new DateOnly(2025, 8, 22) });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "ItemType", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 0, "Campingspot Small", 25m },
                    { 2, 0, "Campingspot Large", 40m },
                    { 3, 0, "Glamping Upgrade", 120m },
                    { 4, 1, "Meal Voucher", 12.5m },
                    { 5, 1, "Drink Pack", 15m },
                    { 6, 1, "Breakfast", 9.5m },
                    { 7, 2, "Parking Day", 10m },
                    { 8, 2, "Parking Weekend", 25m },
                    { 9, 2, "VIP Parking", 50m },
                    { 10, 3, "T-Shirt", 30m },
                    { 11, 3, "Hoodie", 55m },
                    { 12, 3, "Poster", 12m },
                    { 13, 4, "VIP Day", 80m },
                    { 14, 4, "VIP Weekend", 200m },
                    { 15, 4, "Backstage Tour", 150m },
                    { 16, 5, "Locker", 15m },
                    { 17, 5, "Powerbank Rental", 8m },
                    { 18, 5, "Rain Poncho", 5m }
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "FestivalId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Weekend Basic" },
                    { 2, 1, "Weekend Plus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Festivals",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
