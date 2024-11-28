using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddZombieSiege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 226, DateTimeKind.Local).AddTicks(9868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 225, DateTimeKind.Local).AddTicks(5440));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 211, DateTimeKind.Local).AddTicks(7279));

            migrationBuilder.CreateTable(
                name: "ZombieSieges",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AllianceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AllianceSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZombieSieges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZombieSieges_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "dbo",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZombieSiegeParticipants",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZombieSiegeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurvivedWaves = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZombieSiegeParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZombieSiegeParticipants_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "dbo",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZombieSiegeParticipants_ZombieSieges_ZombieSiegeId",
                        column: x => x.ZombieSiegeId,
                        principalSchema: "dbo",
                        principalTable: "ZombieSieges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZombieSiegeParticipants_PlayerId",
                schema: "dbo",
                table: "ZombieSiegeParticipants",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ZombieSiegeParticipants_ZombieSiegeId",
                schema: "dbo",
                table: "ZombieSiegeParticipants",
                column: "ZombieSiegeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZombieSieges_AllianceId",
                schema: "dbo",
                table: "ZombieSieges",
                column: "AllianceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZombieSiegeParticipants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ZombieSieges",
                schema: "dbo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 226, DateTimeKind.Local).AddTicks(9868),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 225, DateTimeKind.Local).AddTicks(5440),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 211, DateTimeKind.Local).AddTicks(7279),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
