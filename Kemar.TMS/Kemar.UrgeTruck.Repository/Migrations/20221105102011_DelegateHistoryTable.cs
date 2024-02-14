using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class DelegateHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTransaction_UserManager_UserManagerId",
                table: "TaskTransaction");

            migrationBuilder.DropIndex(
                name: "IX_TaskTransaction_UserManagerId",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "UserManagerId",
                table: "TaskTransaction");

            migrationBuilder.AddColumn<bool>(
                name: "PushedToDesktop",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushedToMobile",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DelegateHistory",
                columns: table => new
                {
                    delegateHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaisedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    TransferToId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateHistory", x => x.delegateHistoryId);
                    table.ForeignKey(
                        name: "FK_DelegateHistory_TaskTransaction_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskTransaction",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTransaction_AssignedTo",
                table: "TaskTransaction",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateHistory_TaskId",
                table: "DelegateHistory",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTransaction_UserManager_AssignedTo",
                table: "TaskTransaction",
                column: "AssignedTo",
                principalTable: "UserManager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTransaction_UserManager_AssignedTo",
                table: "TaskTransaction");

            migrationBuilder.DropTable(
                name: "DelegateHistory");

            migrationBuilder.DropIndex(
                name: "IX_TaskTransaction_AssignedTo",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "PushedToDesktop",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "PushedToMobile",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "UserManagerId",
                table: "TaskTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTransaction_UserManagerId",
                table: "TaskTransaction",
                column: "UserManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTransaction_UserManager_UserManagerId",
                table: "TaskTransaction",
                column: "UserManagerId",
                principalTable: "UserManager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
