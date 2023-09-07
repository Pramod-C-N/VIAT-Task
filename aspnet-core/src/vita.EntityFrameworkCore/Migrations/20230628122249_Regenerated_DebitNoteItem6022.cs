using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_DebitNoteItem6022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "DebitNoteItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalData2",
                table: "DebitNoteItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DebitNoteItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "DebitNoteItem");

            migrationBuilder.DropColumn(
                name: "AdditionalData2",
                table: "DebitNoteItem");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DebitNoteItem");
        }
    }
}
