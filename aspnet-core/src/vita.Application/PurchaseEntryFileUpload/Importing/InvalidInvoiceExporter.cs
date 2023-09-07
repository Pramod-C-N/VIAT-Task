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
using vita.ImportBatch.Dtos;

namespace vita.PurchaseFileUpload.Importing
{
    public class InvalidInvoiceExporter : NpoiExcelExporterBase, ITransientDependency , IInvalidInvoiceExporter
    {
        public InvalidInvoiceExporter(ITempFileCacheManager tempFileCacheManager)
          : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<CreateOrEditImportBatchDataDto> userListDtos)
        {
            return CreateExcelPackage(
                "InvalidPurchaseImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvalidPurchaseEntryImports");

                    AddHeader(
                        sheet,
                        L("Invoice Type"),
                        L("IRN Number"),
                        L("Invoice Number"),
                        L("Invoice Issue Date"),
                        L("Invoice Issue Time"),
                        L("Invoice Currency Code"),
                        L("Purchase Order Id"),
                        L("Contract Id"),
                        L("Buyer Master Code"),
                        L("Buyer Name"),
                        L("Buyer VAT Number"),
                        L("Buyer Contact"),
                        L("Invoice Line Identifier"),
                        L("Item Master Code"),
                        L("Item Name"),
                        L("UOM"),
                        L("Item Gross Price"),
                        L("Item Price Discount"),
                        L("Item Net Price"),
                        L("Invoiced Quantity"),
                        L("Invoice Line Net Amount"),
                        L("Invoiced Item VAT Category Code"),
                        L("Invoiced Item VAT Rate"),
                        L("VAT Exemption Reason Code"),
                        L("VAT Exemption Reason"),
                        L("VAT Line Amount"),
                        L("Line Amount Inclusive VAT"),
                        L("Message")
                    );

                    AddObjects(
                        sheet, userListDtos,
                        _ => _.InvoiceType,
                        _ => _.IRNNo,
                        _ => _.InvoiceNumber,
                        _ => _.IssueDate,
                        _ => _.IssueTime,
                        _ => _.InvoiceCurrencyCode,
                        _ => _.PurchaseOrderId,
                        _ => _.ContractId,
                        _ => _.BuyerMasterCode,
                        _ => _.BuyerName,
                        _ => _.BuyerVatCode,
                        _ => _.BuyerContact,
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
                        _ => _.LineAmountInclusiveVAT
                      //  _ => _.Exception + _.Error
                    );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
