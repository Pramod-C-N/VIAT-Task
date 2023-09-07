﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_PurchaseDebitNoteVATDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseDebitNoteVATDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IRNNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxSchemeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExcemptionReasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcemptionReasonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseDebitNoteVATDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDebitNoteVATDetail_TenantId",
                table: "PurchaseDebitNoteVATDetail",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseDebitNoteVATDetail");
        }
    }
}
