using Microsoft.EntityFrameworkCore.Migrations;

namespace GiveAwayApp.Migrations
{
    public partial class LodtrækningTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lodtrækning",
                columns: table => new
                {
                    LodtrækningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValgteSpilId = table.Column<int>(type: "int", nullable: false),
                    VinderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ValgtTilLodtrækning = table.Column<bool>(type: "bit", nullable: false),
                    ErTrukket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lodtrækning", x => x.LodtrækningId);
                    table.ForeignKey(
                        name: "FK_Lodtrækning_AspNetUsers_VinderId",
                        column: x => x.VinderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lodtrækning_Spil_ValgteSpilId",
                        column: x => x.ValgteSpilId,
                        principalTable: "Spil",
                        principalColumn: "SpilId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lodtrækning_ValgteSpilId",
                table: "Lodtrækning",
                column: "ValgteSpilId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lodtrækning_VinderId",
                table: "Lodtrækning",
                column: "VinderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lodtrækning");
        }
    }
}
