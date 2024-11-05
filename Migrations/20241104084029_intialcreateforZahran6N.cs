using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class intialcreateforZahran6N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PartnerStoreSubTypes_SubTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubTypeId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "categoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_categoryId",
                table: "Products",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories_categoryId",
                table: "Products",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories_categoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_categoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "SubTypeId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubTypeId",
                table: "Products",
                column: "SubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PartnerStoreSubTypes_SubTypeId",
                table: "Products",
                column: "SubTypeId",
                principalTable: "PartnerStoreSubTypes",
                principalColumn: "Id");
        }
    }
}
