using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiceMaster3600.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UniversityId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UniversityId",
                table: "Users",
                newName: "Gender");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyEntityId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Universities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Faculties",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyEntityId",
                table: "Users",
                column: "FacultyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Faculties_FacultyEntityId",
                table: "Users",
                column: "FacultyEntityId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Faculties_FacultyEntityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FacultyEntityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FacultyEntityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Users",
                newName: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniversityId",
                table: "Users",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
