using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllianceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncryptedKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiKeys_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "dbo",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_AllianceId",
                schema: "dbo",
                table: "ApiKeys",
                column: "AllianceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys",
                schema: "dbo");
        }
    }
}
