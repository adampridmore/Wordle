using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordleAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Guess1 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Guess2 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Guess3 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Guess4 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Guess5 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Guess6 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamId",
                table: "Games",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
