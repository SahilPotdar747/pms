using Microsoft.EntityFrameworkCore.Migrations;

namespace Kemar.TMS.Repository.Migrations
{
    public partial class RaisedByNameIndelageHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransferTo",
                table: "DelegateHistory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferTo",
                table: "DelegateHistory");
        }
    }
}
