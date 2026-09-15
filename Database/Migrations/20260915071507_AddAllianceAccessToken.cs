using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAllianceAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCombatRecord_Players_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCombatRecord",
                schema: "dbo",
                table: "PlayerCombatRecord");

            migrationBuilder.RenameTable(
                name: "PlayerCombatRecord",
                schema: "dbo",
                newName: "PlayerCombatRecords",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerCombatRecord_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecords",
                newName: "IX_PlayerCombatRecords_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCombatRecords",
                schema: "dbo",
                table: "PlayerCombatRecords",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AllianceAccessTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllianceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllianceAccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllianceAccessTokens_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "dbo",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllianceAccessTokens_AllianceId",
                schema: "dbo",
                table: "AllianceAccessTokens",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_AllianceAccessTokens_Token",
                schema: "dbo",
                table: "AllianceAccessTokens",
                column: "Token",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCombatRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecords",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCombatRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecords");

            migrationBuilder.DropTable(
                name: "AllianceAccessTokens",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCombatRecords",
                schema: "dbo",
                table: "PlayerCombatRecords");

            migrationBuilder.RenameTable(
                name: "PlayerCombatRecords",
                schema: "dbo",
                newName: "PlayerCombatRecord",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerCombatRecords_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecord",
                newName: "IX_PlayerCombatRecord_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCombatRecord",
                schema: "dbo",
                table: "PlayerCombatRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCombatRecord_Players_PlayerId",
                schema: "dbo",
                table: "PlayerCombatRecord",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
