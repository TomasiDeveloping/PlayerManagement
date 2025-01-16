using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDismissToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DismissalReason",
                schema: "dbo",
                table: "Players",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissedAt",
                schema: "dbo",
                table: "Players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDismissed",
                schema: "dbo",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01946f11-c5f1-750f-b3f5-61ec7a00f837"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01946f11-c5f1-7576-b861-14df423f92f2"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "VsDuelLeagues",
                keyColumn: "Id",
                keyValue: new Guid("01946f11-c5f1-771e-8600-331582290457"));

            migrationBuilder.DropColumn(
                name: "DismissalReason",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DismissedAt",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsDismissed",
                schema: "dbo",
                table: "Players");

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
    }
}
