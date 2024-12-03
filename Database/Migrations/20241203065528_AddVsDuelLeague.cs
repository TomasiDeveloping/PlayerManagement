using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVsDuelLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VsDuelLeagues",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsDuelLeagues", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "VsDuelLeagues",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01938b4d-6cb9-7813-a409-475c9338f970"), "Gold League" },
                    { new Guid("01938b4d-6cb9-7c03-afef-5767a2132543"), "Diamond League" },
                    { new Guid("01938b4d-6cb9-7e61-b46b-781659ca5694"), "Silver League" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VsDuels_VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels",
                column: "VsDuelLeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_VsDuels_VsDuelLeagues_VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels",
                column: "VsDuelLeagueId",
                principalSchema: "dbo",
                principalTable: "VsDuelLeagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VsDuels_VsDuelLeagues_VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels");

            migrationBuilder.DropTable(
                name: "VsDuelLeagues",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_VsDuels_VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels");

            migrationBuilder.DropColumn(
                name: "VsDuelLeagueId",
                schema: "dbo",
                table: "VsDuels");
        }
    }
}
