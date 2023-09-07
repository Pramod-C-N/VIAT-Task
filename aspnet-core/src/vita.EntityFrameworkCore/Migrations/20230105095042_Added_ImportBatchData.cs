using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_ImportBatchData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportBatchData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IRNNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerMasterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerVatCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerCountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceLineIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemMasterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineNetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatCategoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatExemptionReasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatExemptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATLineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineAmountInclusiveVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrignalSupplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReasonForCN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillOfEntry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillOfEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomsPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WHTApplicable = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvanceRcptAmtAdjusted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatOnAdvanceRcptAmtAdjusted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvanceRcptRefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMeans = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureofServices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isapportionment = table.Column<bool>(type: "bit", nullable: false),
                    ExciseTaxPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherChargesPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VATDeffered = table.Column<bool>(type: "bit", nullable: false),
                    PlaceofSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RCMApplicable = table.Column<bool>(type: "bit", nullable: false),
                    OrgType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AffiliationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceInvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerCapitaHoldingForiegnCo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapitalInvestedbyForeignCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapitalInvestmentCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapitalInvestmentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorCostitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ImportBatchData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatchData_TenantId",
                table: "ImportBatchData",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportBatchData");
        }
    }
}
