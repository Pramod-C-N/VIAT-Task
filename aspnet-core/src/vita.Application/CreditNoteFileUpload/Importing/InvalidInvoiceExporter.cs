using Abp.Collections.Extensions;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users.Importing;
using vita.Authorization.Users.Importing.Dto;
using vita.DataExporting.Excel.NPOI;
using vita.Dto;
//using vita.CreditNoteFileUpload.Dtos;
using vita.Storage;
//using vita.StandardFileUpload.Dtos;
using vita.ImportBatch.Dtos;

namespace vita.CreditNoteFileUpload.Importing
{
    public class InvalidInvoiceExporter : NpoiExcelExporterBase, ITransientDependency, IInvalidInvoiceExporter
    {
        public InvalidInvoiceExporter(ITempFileCacheManager tempFileCacheManager)
          : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<CreateOrEditImportBatchDataDto> userListDtos)
        {
            return CreateExcelPackage(
                     "InvalidPurchaseData.xlsx",
                     excelPackage =>
                     {

                         var sheet = excelPackage.CreateSheet("PurchaseDetail");

                         AddHeader(
                              sheet,
                              L("Purchase Number"),
                              L("Purchase Category"),
                              L("Ledger Head"),
                              L("Supplier Invoice Number"),
                              L("Purchase Date"),
                              L("Purchase Time"),
                              L("Purchase Currency Code"),
                              L("Supplier Invoice Date"),
                              L("Supplier Master Code"),
                              L("Supplier Name"),
                              L("Supplier VAT Number"),
                              L("Supplier Contact"),
                              L("Supplier Country Code"),
                              L("Purchase Entry Line Identifier"),
                              L("Item Master Code"),
                              L("Item Name"),
                              L("UOM"),
                              L("Item Gross Price"),
                              L("Item Price Discount"),
                              L("Item Net Price"),
                              L("Purchased Quantity"),
                              L("Purchase Line Net Amount"),
                              L("Purchase Item VAT Category Code"),
                              L("Purchase Item VAT Rate"),
                              L("VAT Exemption Reason Code"),
                              L("VAT Exemption Reason"),
                              L("VAT Line Amount"),
                              L("Line Amount Inclusive VAT"),
                              L("Bill Of Entry"),
                              L("Bill Of Entry Date"),
                              L("Custom Paid"),
                              L("Custom Tax"),
                              L("WHT Applicable"),
                              L("Message")
                          );

                         AddObjects(
                             sheet, userListDtos,
                             _ => _.PurchaseNumber,
                             _ => _.PurchaseCategory,
                             _ => _.LedgerHeader,
                             _ => _.InvoiceNumber,
                             _ => _.IssueDate?.Date,
                             _ => _.IssueTime,
                             _ => _.InvoiceCurrencyCode,
                             _ => _.OrignalSupplyDate?.Date,
                             _ => _.BuyerMasterCode,
                             _ => _.BuyerName,
                             _ => _.BuyerVatCode,
                             _ => _.BuyerContact,
                             _ => _.BuyerCountryCode,
                             _ => _.InvoiceLineIdentifier,
                             _ => _.ItemMasterCode,
                             _ => _.ItemName,
                             _ => _.UOM,
                             _ => _.GrossPrice,
                             _ => _.Discount,
                             _ => _.NetPrice,
                             _ => _.Quantity,
                             _ => _.LineNetAmount,
                             _ => _.VatCategoryCode,
                             _ => _.VatRate,
                             _ => _.VatExemptionReasonCode,
                             _ => _.VatExemptionReason,
                             _ => _.VATLineAmount,
                             _ => _.BillOfEntry,
                             _ => _.BillOfEntryDate,
                             _ => _.CustomsPaid,
                             _ => _.CustomTax,
                             _ => _.WHTApplicable
                            // _ => _.Error+_.Exception
                         );

                     });

        }
    }
}
