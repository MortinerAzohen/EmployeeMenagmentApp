using Microsoft.EntityFrameworkCore.Migrations;

namespace MyApp.Migrations
{
    public partial class department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepoId",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Depos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepoId",
                table: "Employees",
                column: "DepoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Depos_DepoId",
                table: "Employees",
                column: "DepoId",
                principalTable: "Depos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Depos_DepoId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Depos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepoId",
                table: "Employees");
        }
    }
}
