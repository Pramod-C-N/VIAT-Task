using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_SalesInvoice6528 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Additional_Data",
                table: "SalesInvoice",
                newName: "XmlUuid");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData2",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData3",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData4",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceTypeCode",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "AdditionalData2",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "AdditionalData3",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "AdditionalData4",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "InvoiceTypeCode",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "SalesInvoice");

            migrationBuilder.RenameColumn(
                name: "XmlUuid",
                table: "SalesInvoice",
                newName: "Additional_Data");
        }
    }
}
