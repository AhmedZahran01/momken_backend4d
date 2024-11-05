using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class intialcreateforZahran13N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userName",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "PhoneNumper",
                table: "Clients",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Clients",
                newName: "PhoneNumper");

            migrationBuilder.AddColumn<string>(
                name: "userName",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
