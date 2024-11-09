using System;
using Microsoft.EntityFrameworkCore.Migrations;
using momken_backend.Dtos.Myfatoorah;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class inital4D4s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "myfatoorahClientTempDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<PaidClientOrderTempData>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myfatoorahClientTempDatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "myfatoorahClientTempDatas");
        }
    }
}
