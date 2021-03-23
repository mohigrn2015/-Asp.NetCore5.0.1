using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestProjectIsDB.Migrations
{
    public partial class changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    GradeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.GradeID);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_players_grades_GradeID",
                        column: x => x.GradeID,
                        principalTable: "grades",
                        principalColumn: "GradeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_GradeID",
                table: "players",
                column: "GradeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "grades");
        }
    }
}
