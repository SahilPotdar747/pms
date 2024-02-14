using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class SupportingManagerinUserManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTransaction_ProjectMaster_ProjectId",
                table: "TaskTransaction");

            migrationBuilder.AlterColumn<int>(
                name: "ReportingUser",
                table: "UserManager",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SupportingManager",
                table: "UserManager",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTransaction_ProjectMaster_ProjectId",
                table: "TaskTransaction",
                column: "ProjectId",
                principalTable: "ProjectMaster",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTransaction_ProjectMaster_ProjectId",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "SupportingManager",
                table: "UserManager");

            migrationBuilder.AlterColumn<int>(
                name: "ReportingUser",
                table: "UserManager",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTransaction_ProjectMaster_ProjectId",
                table: "TaskTransaction",
                column: "ProjectId",
                principalTable: "ProjectMaster",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
