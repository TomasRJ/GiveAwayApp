using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GiveAwayApp.Migrations
{
    public partial class Statistik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistik",
                columns: table => new
                {
                    StatistikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntalBesøgere = table.Column<int>(type: "int", nullable: false),
                    AntalBesøgereForDato = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistik", x => x.StatistikId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistik");
        }
    }
}
