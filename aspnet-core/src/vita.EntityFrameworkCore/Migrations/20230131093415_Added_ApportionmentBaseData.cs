using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_ApportionmentBaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApportionmentBaseData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveFromDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveTillEndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxableSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxablePurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApportionmentBaseData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApportionmentBaseData_TenantId",
                table: "ApportionmentBaseData",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApportionmentBaseData");
        }
    }
}
