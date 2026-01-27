using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciandt.Retail.MCP.Migrations
{
    /// <inheritdoc />
    public partial class gender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Customers",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2875));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2876));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Gender" },
                values: new object[] { new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2845), "" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Gender" },
                values: new object[] { new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2847), "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "IMP-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "NOT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2682));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-002",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "TAB-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 19, 17, 44, 170, DateTimeKind.Utc).AddTicks(2685));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9606));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "IMP-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "NOT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-002",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "TAB-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9451));
        }
    }
}
