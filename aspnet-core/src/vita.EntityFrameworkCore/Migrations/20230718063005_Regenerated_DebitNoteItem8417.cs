using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_DebitNoteItem8417 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isOtherCharges",
                table: "DebitNoteItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOtherCharges",
                table: "DebitNoteItem");
        }
    }
}
