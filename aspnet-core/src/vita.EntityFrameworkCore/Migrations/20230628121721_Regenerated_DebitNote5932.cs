using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_DebitNote5932 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Additional_Data",
                table: "DebitNote",
                newName: "XmlUuid");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData2",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData3",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData4",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceTypeCode",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DebitNote",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "DebitNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData2",
                table: "DebitNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData3",
                table: "DebitNote");

            migrationBuilder.DropColumn(
                name: "AdditionalData4",
                table: "DebitNote");

            migrationBuilder.DropColumn(
                name: "InvoiceTypeCode",
                table: "DebitNote");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DebitNote");

            migrationBuilder.RenameColumn(
                name: "XmlUuid",
                table: "DebitNote",
                newName: "Additional_Data");
        }
    }
}
