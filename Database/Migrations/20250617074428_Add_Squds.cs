using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Squds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "SquadType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Squad",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SquadTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Power = table.Column<long>(type: "bigint", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Squad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Squad_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "dbo",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Squad_SquadType_SquadTypeId",
                        column: x => x.SquadTypeId,
                        principalSchema: "dbo",
                        principalTable: "SquadType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SquadType",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { new Guid("01977cd8-bb62-7089-85cb-5a48223a6e92"), "Aircraft" },
                    { new Guid("01977cd8-bb62-7150-a0f9-5415e46a87e4"), "Mixed" },
                    { new Guid("01977cd8-bb62-79aa-9a71-95d57250d723"), "Missile" },
                    { new Guid("01977cd8-bb62-7d5b-823e-b77c6121c4f1"), "Tanks" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Squad_PlayerId",
                schema: "dbo",
                table: "Squad",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Squad_SquadTypeId",
                schema: "dbo",
                table: "Squad",
                column: "SquadTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Squad",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SquadType",
                schema: "dbo");
        }
    }
}
