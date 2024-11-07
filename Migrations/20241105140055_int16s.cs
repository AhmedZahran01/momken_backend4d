using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int16s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrdersClientId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderTotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrdersClientId",
                table: "Products",
                column: "OrdersClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_orders_OrdersClientId",
                table: "Products",
                column: "OrdersClientId",
                principalTable: "orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_orders_OrdersClientId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrdersClientId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrdersClientId",
                table: "Products");
        }
    }
}
