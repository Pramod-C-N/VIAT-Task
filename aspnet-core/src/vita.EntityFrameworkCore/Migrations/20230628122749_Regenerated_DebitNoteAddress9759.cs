using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_DebitNoteAddress9759 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData1",
                table: "DebitNoteAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DebitNoteAddress",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData1",
                table: "DebitNoteAddress");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DebitNoteAddress");
        }
    }
}
