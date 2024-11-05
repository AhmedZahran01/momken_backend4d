using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class intialcreateforZahran5N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerStores_PartnerStoreSubTypes_SubTypeId",
                table: "PartnerStores");

            migrationBuilder.DropIndex(
                name: "IX_PartnerStores_SubTypeId",
                table: "PartnerStores");

            migrationBuilder.DropColumn(
                name: "SubTypeId",
                table: "PartnerStores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubTypeId",
                table: "PartnerStores",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerStores_SubTypeId",
                table: "PartnerStores",
                column: "SubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerStores_PartnerStoreSubTypes_SubTypeId",
                table: "PartnerStores",
                column: "SubTypeId",
                principalTable: "PartnerStoreSubTypes",
                principalColumn: "Id");
        }
    }
}
