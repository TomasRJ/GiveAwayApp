using Microsoft.EntityFrameworkCore.Migrations;

namespace GiveAwayApp.Migrations
{
    public partial class LodtrækningTabelMedBrugerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VinderBrugerId",
                table: "Lodtrækning",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VinderBrugerId",
                table: "Lodtrækning");
        }
    }
}
