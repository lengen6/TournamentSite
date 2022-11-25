using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TieRenTournament.Migrations
{
    public partial class UpdatedMatchModelQuatro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bracket",
                table: "Match",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bracket",
                table: "Match");
        }
    }
}
