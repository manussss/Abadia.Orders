using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abadia.Orders.Infra.Migrations
{
    public partial class RefactorDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "OrderUpload");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OrderItem",
                newName: "TaxValue");

            migrationBuilder.RenameColumn(
                name: "CreditDate",
                table: "OrderItem",
                newName: "ReceiveDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "OrderItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Product",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "TaxValue",
                table: "OrderItem",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ReceiveDate",
                table: "OrderItem",
                newName: "CreditDate");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "OrderUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Taxes",
                table: "OrderItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Product",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Taxes",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
