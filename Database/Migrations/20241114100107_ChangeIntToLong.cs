﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIntToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "WeeklyPoints",
                schema: "dbo",
                table: "VsDuelParticipants",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 162, DateTimeKind.Local).AddTicks(7710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 96, DateTimeKind.Local).AddTicks(5768));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 161, DateTimeKind.Local).AddTicks(4992),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 95, DateTimeKind.Local).AddTicks(4388));

            migrationBuilder.AlterColumn<long>(
                name: "AchievedPoints",
                schema: "dbo",
                table: "CustomEventParticipants",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 149, DateTimeKind.Local).AddTicks(7087),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 84, DateTimeKind.Local).AddTicks(3748));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WeeklyPoints",
                schema: "dbo",
                table: "VsDuelParticipants",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 96, DateTimeKind.Local).AddTicks(5768),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 162, DateTimeKind.Local).AddTicks(7710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 95, DateTimeKind.Local).AddTicks(4388),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 161, DateTimeKind.Local).AddTicks(4992));

            migrationBuilder.AlterColumn<int>(
                name: "AchievedPoints",
                schema: "dbo",
                table: "CustomEventParticipants",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "dbo",
                table: "Alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 42, 40, 84, DateTimeKind.Local).AddTicks(3748),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 1, 7, 149, DateTimeKind.Local).AddTicks(7087));
        }
    }
}