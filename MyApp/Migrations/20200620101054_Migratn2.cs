using Microsoft.EntityFrameworkCore.Migrations;

namespace MyApp.Migrations
{
    public partial class Migratn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "Mark2@gmail.com", "Mark2" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Departmennt", "Email", "Name", "Surname" },
                values: new object[] { 2, 1, "John@gmail.com", "John", "Klark" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "Mark@gmail.com", "Mark" });
        }
    }
}
