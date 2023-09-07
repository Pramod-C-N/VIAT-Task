using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Regenerated_ApportionmentBaseData7455 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ApportionmentPurchases",
                table: "ApportionmentBaseData",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ApportionmentSupplies",
                table: "ApportionmentBaseData",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MixedOverHeads",
                table: "ApportionmentBaseData",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApportionmentPurchases",
                table: "ApportionmentBaseData");

            migrationBuilder.DropColumn(
                name: "ApportionmentSupplies",
                table: "ApportionmentBaseData");

            migrationBuilder.DropColumn(
                name: "MixedOverHeads",
                table: "ApportionmentBaseData");
        }
    }
}
