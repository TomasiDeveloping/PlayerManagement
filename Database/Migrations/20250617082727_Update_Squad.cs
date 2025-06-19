using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Squad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Squad_Players_PlayerId",
                schema: "dbo",
                table: "Squad");

            migrationBuilder.DropForeignKey(
                name: "FK_Squad_SquadType_SquadTypeId",
                schema: "dbo",
                table: "Squad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SquadType",
                schema: "dbo",
                table: "SquadType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Squad",
                schema: "dbo",
                table: "Squad");


            migrationBuilder.RenameTable(
                name: "SquadType",
                schema: "dbo",
                newName: "SquadTypes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Squad",
                schema: "dbo",
                newName: "Squads",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Squad_SquadTypeId",
                schema: "dbo",
                table: "Squads",
                newName: "IX_Squads_SquadTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Squad_PlayerId",
                schema: "dbo",
                table: "Squads",
                newName: "IX_Squads_PlayerId");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "dbo",
                table: "Squads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SquadTypes",
                schema: "dbo",
                table: "SquadTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Squads",
                schema: "dbo",
                table: "Squads",
                column: "Id");


            migrationBuilder.AddForeignKey(
                name: "FK_Squads_Players_PlayerId",
                schema: "dbo",
                table: "Squads",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squads_SquadTypes_SquadTypeId",
                schema: "dbo",
                table: "Squads",
                column: "SquadTypeId",
                principalSchema: "dbo",
                principalTable: "SquadTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Squads_Players_PlayerId",
                schema: "dbo",
                table: "Squads");

            migrationBuilder.DropForeignKey(
                name: "FK_Squads_SquadTypes_SquadTypeId",
                schema: "dbo",
                table: "Squads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SquadTypes",
                schema: "dbo",
                table: "SquadTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Squads",
                schema: "dbo",
                table: "Squads");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SquadTypes",
                keyColumn: "Id",
                keyValue: new Guid("01977d00-1596-7078-b24d-f5abd8baaec1"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SquadTypes",
                keyColumn: "Id",
                keyValue: new Guid("01977d00-1596-71a1-bbff-db88a4a59f32"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SquadTypes",
                keyColumn: "Id",
                keyValue: new Guid("01977d00-1596-7644-98aa-ff20f05f13bb"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SquadTypes",
                keyColumn: "Id",
                keyValue: new Guid("01977d00-1596-7c18-a665-e079870ae3cb"));

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "dbo",
                table: "Squads");

            migrationBuilder.RenameTable(
                name: "SquadTypes",
                schema: "dbo",
                newName: "SquadType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Squads",
                schema: "dbo",
                newName: "Squad",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Squads_SquadTypeId",
                schema: "dbo",
                table: "Squad",
                newName: "IX_Squad_SquadTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Squads_PlayerId",
                schema: "dbo",
                table: "Squad",
                newName: "IX_Squad_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SquadType",
                schema: "dbo",
                table: "SquadType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Squad",
                schema: "dbo",
                table: "Squad",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Squad_Players_PlayerId",
                schema: "dbo",
                table: "Squad",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squad_SquadType_SquadTypeId",
                schema: "dbo",
                table: "Squad",
                column: "SquadTypeId",
                principalSchema: "dbo",
                principalTable: "SquadType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
