﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeToVsDuelLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "dbo",
                table: "VsDuelLeagues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "VsDuelLeagues",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("01938bf2-cf1b-742f-b3d6-824dbed7bf25"), 1, "Silver League" },
                    { new Guid("01938bf2-cf1b-7a69-8607-87b1987a19b0"), 2, "Gold League" },
                    { new Guid("01938bf2-cf1b-7cde-961e-e8cb35890551"), 3, "Diamond League" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938bf2-cf1b-742f-b3d6-824dbed7bf25"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938bf2-cf1b-7a69-8607-87b1987a19b0"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01938bf2-cf1b-7cde-961e-e8cb35890551"));

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "dbo",
                table: "VsDuelLeagues");

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
    }
}