using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class coordinatingInchargeindepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingManager",
                table: "UserManager");

            migrationBuilder.AddColumn<int>(
                name: "taskNumber",
                table: "TaskTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "coordinatingIncharge",
                table: "DepartmentMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coordinatingInchargeName",
                table: "DepartmentMaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taskNumber",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "coordinatingIncharge",
                table: "DepartmentMaster");

            migrationBuilder.DropColumn(
                name: "coordinatingInchargeName",
                table: "DepartmentMaster");

            migrationBuilder.AddColumn<int>(
                name: "SupportingManager",
                table: "UserManager",
                type: "int",
                nullable: true);
        }
    }
}
