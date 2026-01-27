using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciandt.Retail.MCP.Migrations
{
    /// <inheritdoc />
    public partial class documentcpf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentCPF",
                table: "Customers",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "CreatedAt", "DocumentCPF" },
                values: new object[] { new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9605), "" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DocumentCPF" },
                values: new object[] { new DateTime(2026, 1, 20, 17, 34, 49, 940, DateTimeKind.Utc).AddTicks(9606), "" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentCPF",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(1043));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(1044));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(1046));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(1014));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(1016));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "IMP-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(813));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "NOT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(850));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "ROT-002",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(808));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "TAB-001",
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 29, 32, 814, DateTimeKind.Utc).AddTicks(852));
        }
    }
}
