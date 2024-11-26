using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddStartPlayerFlagToDesertStormParticipant : Migration
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
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 226, DateTimeKind.Local).AddTicks(9868),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 162, DateTimeKind.Local).AddTicks(7710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 225, DateTimeKind.Local).AddTicks(5440),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 161, DateTimeKind.Local).AddTicks(4992));

            migrationBuilder.AddColumn<bool>(
                name: "StartPlayer",
                schema: "dbo",
                table: "DesertStormParticipants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 211, DateTimeKind.Local).AddTicks(7279),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 149, DateTimeKind.Local).AddTicks(7087));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartPlayer",
                schema: "dbo",
                table: "DesertStormParticipants");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 162, DateTimeKind.Local).AddTicks(7710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 226, DateTimeKind.Local).AddTicks(9868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 161, DateTimeKind.Local).AddTicks(4992),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 225, DateTimeKind.Local).AddTicks(5440));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 149, DateTimeKind.Local).AddTicks(7087),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 20, 10, 20, 41, 211, DateTimeKind.Local).AddTicks(7279));
        }
    }
}
