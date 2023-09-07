using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_CreditNote4509 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Additional_Data",
                table: "CreditNote",
                newName: "XmlUuid");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData2",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData3",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData4",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceTypeCode",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CreditNote",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "CreditNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData2",
                table: "CreditNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData3",
                table: "CreditNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData4",
                table: "CreditNote");

            migrationBuilder.DropColumn(
                name: "InvoiceTypeCode",
                table: "CreditNote");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CreditNote");

            migrationBuilder.RenameColumn(
                name: "XmlUuid",
                table: "CreditNote",
                newName: "Additional_Data");
        }
    }
}
