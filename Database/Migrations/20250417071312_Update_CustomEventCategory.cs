﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_CustomEventCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventCategory_Alliances_AllianceId",
                schema: "dbo",
                table: "CustomEventCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomEvents_CustomEventCategory_CustomEventCategoryId",
                schema: "dbo",
                table: "CustomEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomEventCategory",
                schema: "dbo",
                table: "CustomEventCategory");

            migrationBuilder.RenameTable(
                name: "CustomEventCategory",
                schema: "dbo",
                newName: "CustomEventCategories",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEventCategory_AllianceId",
                schema: "dbo",
                table: "CustomEventCategories",
                newName: "IX_CustomEventCategories_AllianceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomEventCategories",
                schema: "dbo",
                table: "CustomEventCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventCategories_Alliances_AllianceId",
                schema: "dbo",
                table: "CustomEventCategories",
                column: "AllianceId",
                principalSchema: "dbo",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEvents_CustomEventCategories_CustomEventCategoryId",
                schema: "dbo",
                table: "CustomEvents",
                column: "CustomEventCategoryId",
                principalSchema: "dbo",
                principalTable: "CustomEventCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventCategories_Alliances_AllianceId",
                schema: "dbo",
                table: "CustomEventCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomEvents_CustomEventCategories_CustomEventCategoryId",
                schema: "dbo",
                table: "CustomEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomEventCategories",
                schema: "dbo",
                table: "CustomEventCategories");

            migrationBuilder.RenameTable(
                name: "CustomEventCategories",
                schema: "dbo",
                newName: "CustomEventCategory",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEventCategories_AllianceId",
                schema: "dbo",
                table: "CustomEventCategory",
                newName: "IX_CustomEventCategory_AllianceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomEventCategory",
                schema: "dbo",
                table: "CustomEventCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventCategory_Alliances_AllianceId",
                schema: "dbo",
                table: "CustomEventCategory",
                column: "AllianceId",
                principalSchema: "dbo",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEvents_CustomEventCategory_CustomEventCategoryId",
                schema: "dbo",
                table: "CustomEvents",
                column: "CustomEventCategoryId",
                principalSchema: "dbo",
                principalTable: "CustomEventCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
