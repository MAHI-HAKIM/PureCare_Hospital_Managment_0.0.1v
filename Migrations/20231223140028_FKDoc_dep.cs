using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureCareHub_HospitalCare.Migrations
{
    public partial class FKDoc_dep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "depID",
                table: "doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_doctors_depID",
                table: "doctors",
                column: "depID");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_Departments_depID",
                table: "doctors",
                column: "depID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_Departments_depID",
                table: "doctors");

            migrationBuilder.DropIndex(
                name: "IX_doctors_depID",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "depID",
                table: "doctors");
        }
    }
}
