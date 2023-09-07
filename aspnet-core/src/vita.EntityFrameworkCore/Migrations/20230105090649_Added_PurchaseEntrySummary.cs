using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_PurchaseEntrySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseEntrySummary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IRNNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetInvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetInvoiceAmountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SumOfInvoiceLineNetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SumOfInvoiceLineNetAmountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmountWithoutVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountWithoutVATCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalVATAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmountWithVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayableAmountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvanceAmountwithoutVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvanceVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_PurchaseEntrySummary", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntrySummary_TenantId",
                table: "PurchaseEntrySummary",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseEntrySummary");
        }
    }
}
