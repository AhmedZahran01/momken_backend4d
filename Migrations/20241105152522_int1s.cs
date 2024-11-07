using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int1s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Clients_clientId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_PartnerStores_partnerStoreId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Reviewsc");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_partnerStoreId",
                table: "Reviewsc",
                newName: "IX_Reviewsc_partnerStoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc",
                columns: new[] { "clientId", "partnerStoreId" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewsc_partnerStoreId",
                table: "Reviews",
                newName: "IX_Reviews_partnerStoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                columns: new[] { "clientId", "partnerStoreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Clients_clientId",
                table: "Reviews",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_PartnerStores_partnerStoreId",
                table: "Reviews",
                column: "partnerStoreId",
                principalTable: "PartnerStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
