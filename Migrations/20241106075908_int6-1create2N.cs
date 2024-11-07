using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int61create2N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Reviewsc",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "EvaluationNumber",
                table: "Reviewsc",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Reviewsc",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviewsc_clientId",
                table: "Reviewsc",
                column: "clientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc");

            migrationBuilder.DropIndex(
                name: "IX_Reviewsc_clientId",
                table: "Reviewsc");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Reviewsc");

            migrationBuilder.DropColumn(
                name: "EvaluationNumber",
                table: "Reviewsc");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Reviewsc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewsc",
                table: "Reviewsc",
                columns: new[] { "clientId", "partnerStoreId" });
        }
    }
}
