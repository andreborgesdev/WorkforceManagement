using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkforceManagement.Migrations
{
    public partial class AddDeathOfDeathToPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfDeath",
                table: "People",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Materials",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "People");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }
    }
}
