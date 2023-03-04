using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abadia.Orders.Infra.Migrations
{
    public partial class AlterTable_OrderUpload_AddFileColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "OrderUpload",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "OrderUpload");
        }
    }
}
