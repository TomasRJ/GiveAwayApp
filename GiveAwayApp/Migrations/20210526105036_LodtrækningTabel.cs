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
                    ValgtTilLodtrækning = table.Column<bool>(type: "bit", nullable: false),
                    ErTrukket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lodtrækning", x => x.LodtrækningId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lodtrækning");
        }
    }
}
