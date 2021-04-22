using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GiveAwayApp.Migrations
{
    public partial class SpilPlusMellemtabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spil",
                columns: table => new
                {
                    SpilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SteamId = table.Column<int>(type: "int", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpilCoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Udgivelsesdato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValgtAntal = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spil", x => x.SpilId);
                });

            migrationBuilder.CreateTable(
                name: "BrugereSpil",
                columns: table => new
                {
                    BrugerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpilId = table.Column<int>(type: "int", nullable: false),
                    OprettelsesDato = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrugereSpil", x => new { x.BrugerId, x.SpilId });
                    table.ForeignKey(
                        name: "FK_BrugereSpil_AspNetUsers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrugereSpil_Spil_SpilId",
                        column: x => x.SpilId,
                        principalTable: "Spil",
                        principalColumn: "SpilId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrugereSpil_SpilId",
                table: "BrugereSpil",
                column: "SpilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrugereSpil");

            migrationBuilder.DropTable(
                name: "Spil");
        }
    }
}
