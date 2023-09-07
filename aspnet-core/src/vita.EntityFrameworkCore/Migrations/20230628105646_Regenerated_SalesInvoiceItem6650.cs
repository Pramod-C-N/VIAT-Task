using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_SalesInvoiceItem6650 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "SalesInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData2",
                table: "SalesInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "SalesInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "SalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "AdditionalData2",
                table: "SalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "SalesInvoiceItem");
        }
    }
}
