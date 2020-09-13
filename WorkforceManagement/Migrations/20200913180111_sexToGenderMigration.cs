using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkforceManagement.Migrations
{
    public partial class sexToGenderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "People");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "People",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("40ff5488-fdab-45b5-bc3a-14302d59869b"),
                column: "Gender",
                value: "Male");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "People");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("40ff5488-fdab-45b5-bc3a-14302d59869b"),
                column: "Sex",
                value: "Male");
        }
    }
}
