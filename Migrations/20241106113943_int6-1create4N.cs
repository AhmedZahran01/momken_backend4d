using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class int61create4N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "orders");

            migrationBuilder.AddColumn<int>(
                name: "orderStatus",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orderStatus",
                table: "orders");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
