using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TieRenTournament.Migrations
{
    public partial class UpdatedMatchModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Round",
                table: "Match",
                newName: "VictoryMethod");

            migrationBuilder.AddColumn<int>(
                name: "RoundNumber",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartingLength",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundNumber",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "StartingLength",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "VictoryMethod",
                table: "Match",
                newName: "Round");
        }
    }
}
