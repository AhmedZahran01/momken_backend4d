using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int61create3N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewsc_Clients_clientId",
                table: "Reviewsc");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviewsc_PartnerStores_partnerStoreId",
                table: "Reviewsc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc");

            migrationBuilder.RenameTable(
                name: "Reviewsc",
                newName: "ReviewsOfClient");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewsc_partnerStoreId",
                table: "ReviewsOfClient",
                newName: "IX_ReviewsOfClient_partnerStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewsc_clientId",
                table: "ReviewsOfClient",
                newName: "IX_ReviewsOfClient_clientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewsOfClient",
                table: "ReviewsOfClient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsOfClient_Clients_clientId",
                table: "ReviewsOfClient",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsOfClient_PartnerStores_partnerStoreId",
                table: "ReviewsOfClient",
                column: "partnerStoreId",
                principalTable: "PartnerStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsOfClient_Clients_clientId",
                table: "ReviewsOfClient");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsOfClient_PartnerStores_partnerStoreId",
                table: "ReviewsOfClient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewsOfClient",
                table: "ReviewsOfClient");

            migrationBuilder.RenameTable(
                name: "ReviewsOfClient",
                newName: "Reviewsc");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewsOfClient_partnerStoreId",
                table: "Reviewsc",
                newName: "IX_Reviewsc_partnerStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewsOfClient_clientId",
                table: "Reviewsc",
                newName: "IX_Reviewsc_clientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewsc_Clients_clientId",
                table: "Reviewsc",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewsc_PartnerStores_partnerStoreId",
                table: "Reviewsc",
                column: "partnerStoreId",
                principalTable: "PartnerStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
