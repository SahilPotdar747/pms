using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class actionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "TaskTypeMaster",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "TaskTypeMaster",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "TaskTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextTaskId",
                table: "TaskTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextTaskname",
                table: "TaskTypeMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "TaskTransaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeMaster_DepartmentId",
                table: "TaskTypeMaster",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTypeMaster_DepartmentMaster_DepartmentId",
                table: "TaskTypeMaster",
                column: "DepartmentId",
                principalTable: "DepartmentMaster",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTypeMaster_DepartmentMaster_DepartmentId",
                table: "TaskTypeMaster");

            migrationBuilder.DropIndex(
                name: "IX_TaskTypeMaster_DepartmentId",
                table: "TaskTypeMaster");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "TaskTypeMaster");

            migrationBuilder.DropColumn(
                name: "NextTaskId",
                table: "TaskTypeMaster");

            migrationBuilder.DropColumn(
                name: "NextTaskname",
                table: "TaskTypeMaster");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "TaskTypeMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "TaskTypeMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "TaskTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
