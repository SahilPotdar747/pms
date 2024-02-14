using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class DepartmentIdInTaskTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "TaskTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTransaction_DepartmentId",
                table: "TaskTransaction",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTransaction_DepartmentMaster_DepartmentId",
                table: "TaskTransaction",
                column: "DepartmentId",
                principalTable: "DepartmentMaster",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTransaction_DepartmentMaster_DepartmentId",
                table: "TaskTransaction");

            migrationBuilder.DropIndex(
                name: "IX_TaskTransaction_DepartmentId",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "TaskTransaction");
        }
    }
}
