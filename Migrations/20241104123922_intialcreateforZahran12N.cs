using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class intialcreateforZahran12N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "partnerStoreId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_partnerStoreId",
                table: "Products",
                column: "partnerStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PartnerStores_partnerStoreId",
                table: "Products",
                column: "partnerStoreId",
                principalTable: "PartnerStores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PartnerStores_partnerStoreId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_partnerStoreId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "partnerStoreId",
                table: "Products");
        }
    }
}
