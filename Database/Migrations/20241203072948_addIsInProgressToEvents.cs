﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class addIsInProgressToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b4d-6cb9-7813-a409-475c9338f970"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b4d-6cb9-7c03-afef-5767a2132543"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b4d-6cb9-7e61-b46b-781659ca5694"));

            migrationBuilder.AddColumn<bool>(
                name: "IsInProgress",
                schema: "dbo",
                table: "VsDuels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInProgress",
                schema: "dbo",
                table: "DesertStorms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInProgress",
                schema: "dbo",
                table: "CustomEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "VsDuelLeagues",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01938b6c-dd30-7182-9fe6-9df201e0586d"), "Silver League" },
                    { new Guid("01938b6c-dd30-7a97-9bfc-e4f49c245b7a"), "Gold League" },
                    { new Guid("01938b6c-dd30-7b55-8254-12337beebc73"), "Diamond League" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b6c-dd30-7182-9fe6-9df201e0586d"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b6c-dd30-7a97-9bfc-e4f49c245b7a"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938b6c-dd30-7b55-8254-12337beebc73"));

            migrationBuilder.DropColumn(
                name: "IsInProgress",
                schema: "dbo",
                table: "VsDuels");

            migrationBuilder.DropColumn(
                name: "IsInProgress",
                schema: "dbo",
                table: "DesertStorms");

            migrationBuilder.DropColumn(
                name: "IsInProgress",
                schema: "dbo",
                table: "CustomEvents");

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
        }
    }
}