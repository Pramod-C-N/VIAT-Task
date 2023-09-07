using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_PurchaseEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseEntry",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IRNNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfSupply = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyCodeOriginatingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatestDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Additional_Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XMLUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArchivalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousInvoiceHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerviousXMLHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XMLHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XMLbase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    TransTypeCode = table.Column<int>(type: "int", nullable: false),
                    TransTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvanceReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoicetransactioncode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessProcessType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierInvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillOfEntry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillOfEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomsPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsWHT = table.Column<bool>(type: "bit", nullable: false),
                    VATDeffered = table.Column<bool>(type: "bit", nullable: false),
                    PlaceofSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseEntry", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntry_TenantId",
                table: "PurchaseEntry",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseEntry");
        }
    }
}
