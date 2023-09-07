using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users.Dto;
using vita.DataExporting.Excel.NPOI;
using vita.Dto;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Exporting;
using vita.ImportBatch.Importing;
using vita.Storage;

namespace vita.ImportBatch.Exporting
{
    public class SalesFileExcelExporter : NpoiExcelExporterBase, ISalesFileExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SalesFileExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(DataTable dt,string fileName)
        {
            return CreateExcelPackage(
                    fileName,
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("Sheet1");
                        string[] row = new string[dt.Columns.Count];
                        for (var i = 0; i < dt.Columns.Count; i++)
                            row[i] = dt.Columns[i].ColumnName;

                        AddHeader(sheet, row);

                        AddObjectsFromDatatable(sheet, dt);
                    });

        }

        public FileDto ExportToFileWithCustomHeader(DataTable dt, string fileName,string header)
        {
            return CreateExcelPackage(
                    fileName,
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("Sheet1");
                        string[] row = new string[dt.Columns.Count];
                        for (var i = 0; i < dt.Columns.Count; i++)
                            row[i] = dt.Columns[i].ColumnName;

                        AddHeader(sheet, header, row);

                        AddObjectsFromDatatable(sheet, dt,2);
                    });

        }



        public FileDto ExportToFile(List<ImportBatchDataDto> paymentDetails)
        {
            if (paymentDetails.FirstOrDefault().InvoiceType.StartsWith("S"))
            {
                return CreateExcelPackage(
                    "InvalidSalesData.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("SalesDetail");

                        AddHeader(
                             sheet,
                             L("Invoice Type"),
                             L("IRN Number"),
                             L("Invoice Number"),
                             //L("Invoice Issue Date"),
                             L("Invoice Issue Time"),
                             L("Invoice Currency Code"),
                             L("Purchase Order Id"),
                             L("Contract Id"),
                             //L("Supply Date"),
                             //L("Supply End Date"),
                             L("Buyer Master Code"),
                             L("Buyer Name"),
                             L("Buyer VAT Number"),
                             L("Buyer Contact"),
                             L("Buyer Country Code"),
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
                            sheet, paymentDetails,
                            _ => _.InvoiceType.Replace("Sales Invoice - ",""),
                            _ => _.IRNNo,
                            _ => _.InvoiceNumber,
                            //_ => _.IssueDate?.Date.ToString("yyyy-MM-dd"),
                            _ => _.IssueTime,
                            _ => _.InvoiceCurrencyCode,
                            _ => _.PurchaseOrderId,
                            _ => _.ContractId,
                            //_ => _.SupplyDate?.Date.ToString("yyyy-MM-dd"),
                            //_ => _.SupplyEndDate?.Date.ToString("yyyy-MM-dd"),
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
                            _ => _.LineAmountInclusiveVAT,
                            _ => _.Error
                        );

                    });
            }
            else if (paymentDetails.FirstOrDefault().InvoiceType.StartsWith("P"))
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
                            sheet, paymentDetails,
                            _ => _.PurchaseNumber,
                            _ => _.PurchaseCategory,
                            _ => _.LedgerHeader,
                            _ => _.InvoiceNumber,
                            //_ => _.IssueDate?.Date,
                            _ => _.IssueTime,
                            _ => _.InvoiceCurrencyCode,
                            //_ => _.OrignalSupplyDate?.Date,
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
                            _ => _.WHTApplicable,
                            _ => _.Error
                        );

                    });
            }
            else
            {
                return CreateExcelPackage(
                    "InvalidCreditNoteData.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("CreditNoteDetail");

                        AddHeader(
                             sheet,
                             L("Credit Note Type"),
                             L("IRN Number"),
                             L("Credit Note Number"),
                             L("Credit Note Issue Date"),
                             L("Credit Note Issue Time"),
                             L("Credit Note Currency Code"),
                             L("Billing Reference Id"),
                             L("Orignal Supply Date"),
                             L("Reason for Issuance of Credit Note"),
                             L("Purchase Order Id"),
                             L("Contract Id"),
                             L("Buyer Master Code"),
                             L("Buyer Name"),
                             L("Buyer VAT Number"),
                             L("Buyer Contact"),
                             L("Buyer Country Code"),
                             L("Credit Note Line Identifier"),
                             L("Item Master Code"),
                             L("Item Name"),
                             L("UOM"),
                             L("Item Gross Price"),
                             L("Item Price Discount"),
                             L("Item Net Price"),
                             L("Credit Note Quantity"),
                             L("Credit Note Line Net Amount"),
                             L("Credit Note Item VAT Category Code"),
                             L("Credit Note Item VAT Rate"),
                             L("VAT Exemption Reason Code"),
                             L("VAT Exemption Reason"),
                             L("VAT Line Amount"),
                             L("Line Amount Inclusive VAT"),
                             L("Message")
                         );

                        AddObjects(
                            sheet, paymentDetails,
                            _ => _.InvoiceType.Replace("Credit Note - ", ""),
                            _ => _.IRNNo,
                            _ => _.InvoiceNumber,
                            _ => _.IssueDate,
                            _ => _.IssueTime,
                            _ => _.InvoiceCurrencyCode,
                            _ => _.BillingReferenceId,
                            _ => _.OrignalSupplyDate,
                            _ => _.ReasonForCN,
                            _ => _.PurchaseOrderId,
                            _ => _.ContractId,
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
                            _ => _.LineAmountInclusiveVAT,
                            _ => _.Error
                        );

                    });
            }
        }
    }
}
