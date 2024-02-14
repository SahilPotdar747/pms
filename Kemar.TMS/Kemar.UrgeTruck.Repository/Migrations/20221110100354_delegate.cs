using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class @delegate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TaskTransaction",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DelegateHistory",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DelegateHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "DelegateHistory",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "DelegateHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectRemarks",
                table: "DelegateHistory",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TaskTransaction");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DelegateHistory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DelegateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "DelegateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DelegateHistory");

            migrationBuilder.DropColumn(
                name: "RejectRemarks",
                table: "DelegateHistory");
        }
    }
}
