using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int14s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PartnerStoreTypes_TypeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Partners_partnerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_partnerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "partnerId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "partnerId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_partnerId",
                table: "Products",
                column: "partnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PartnerStoreTypes_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "PartnerStoreTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Partners_partnerId",
                table: "Products",
                column: "partnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
