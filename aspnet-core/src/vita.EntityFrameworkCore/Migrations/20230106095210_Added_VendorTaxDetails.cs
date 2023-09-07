using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_VendorTaxDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorTaxDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorUniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessSupplies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesVATCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_VendorTaxDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorTaxDetails_TenantId",
                table: "VendorTaxDetails",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorTaxDetails");
        }
    }
}
