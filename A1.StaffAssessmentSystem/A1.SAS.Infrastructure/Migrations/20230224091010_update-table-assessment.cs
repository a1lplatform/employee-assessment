using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A1.SAS.Infrastructure.Migrations
{
    public partial class updatetableassessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Employees_EmployeeId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_EmployeeId",
                table: "Assessments");

            migrationBuilder.AddColumn<Guid>(
                name: "TblEmployeeId",
                table: "Assessments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_TblEmployeeId",
                table: "Assessments",
                column: "TblEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Employees_TblEmployeeId",
                table: "Assessments",
                column: "TblEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Employees_TblEmployeeId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_TblEmployeeId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "TblEmployeeId",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EmployeeId",
                table: "Assessments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Employees_EmployeeId",
                table: "Assessments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
