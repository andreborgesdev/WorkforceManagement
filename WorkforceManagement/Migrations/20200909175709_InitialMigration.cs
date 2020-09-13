using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkforceManagement.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTimeOffset>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: false),
                    NIF = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Sex = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Reference = table.Column<string>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Address", "Contact", "DateOfBirth", "Email", "FirstName", "LastName", "NIF", "Sex" },
                values: new object[] { new Guid("40ff5488-fdab-45b5-bc3a-14302d59869b"), "Tomorrowland", "9123132", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "asd@ad.com", "André", "Borges", "adasd", "Male" });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "PersonId", "Reference" },
                values: new object[] { new Guid("40ff5488-fdab-45b5-bc3a-14302d59869a"), new Guid("40ff5488-fdab-45b5-bc3a-14302d59869b"), "Hammer" });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_PersonId",
                table: "Materials",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
