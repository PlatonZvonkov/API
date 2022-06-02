using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Secondary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Fatherhood = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Strikes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShiftStarts = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShiftEnds = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Hours = table.Column<int>(type: "INTEGER", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Fatherhood", "Name", "Strikes", "Surname", "Title" },
                values: new object[] { 1, "Catz", "Artur", 0, "Kupchinsky", "Manager" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Fatherhood", "Name", "Strikes", "Surname", "Title" },
                values: new object[] { 2, "Galerovich", "Vasya", 0, "Pupkin", "Engineer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Fatherhood", "Name", "Strikes", "Surname", "Title" },
                values: new object[] { 3, "Massonovich", "Grisha", 1, "Zadneprivodniy", "Tester" });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "EmployeeId", "Hours", "ShiftEnds", "ShiftStarts" },
                values: new object[] { 1, 1, 8, new DateTime(2022, 5, 31, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 31, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "EmployeeId", "Hours", "ShiftEnds", "ShiftStarts" },
                values: new object[] { 2, 2, 8, new DateTime(2022, 5, 31, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 31, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "EmployeeId", "Hours", "ShiftEnds", "ShiftStarts" },
                values: new object[] { 3, 3, 1, new DateTime(2022, 5, 31, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 31, 17, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
