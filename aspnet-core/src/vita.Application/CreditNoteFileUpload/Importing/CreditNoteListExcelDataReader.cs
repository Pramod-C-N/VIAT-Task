using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using vita.Authorization.Users.Importing.Dto;
using vita.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;
//using vita.CreditNoteFileUpload.Dtos;
//using vita.StandardFileUpload.Dtos;
using Stripe;
using System.Text.RegularExpressions;
using vita.MultiTenancy.Accounting;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;

namespace vita.CreditNoteFileUpload.Importing
{
    internal class CreditNoteListExcelDataReader : NpoiExcelImporterBase<CreateOrEditImportBatchDataDto>, ICreditNoteListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        public string[] uom = { "LTRS", "PCS", "NOS", "GMS", "KGS", "PACKS" };
        public string[] countryCode = { "AD","AE","AF","AG","AI","AL","AM","AO","AQ","AR","AS","AT","AU","AW","AX","AZ","BA","BB","BD",
"BE","BF","BG","BH","BI","BJ","BL","BM","BN","BO","BQ","BR","BS","BT","BV","BW","BY","BZ","CA",
"CC","CD","CF","CG","CH","CI","CK","CL","CM","CN","CO","CR","CU","CV","CW","CX","CY","CZ","DE",
"DJ","DK","DM","DO","DZ","EC","EE","EG","EH","ER","ES","ET","FI","FJ","FK","FM","FO","FR","GA",
"GB","GD","GE","GF","GG","GH","GI","GL","GM","GN","GP","GQ","GR","GS","GT","GU","GW","GY","HK",
"HM","HN","HR","HT","HU","ID","IE","IL","IM","IN","IO","IQ","IR","IS","IT","JE","JM","JO","JP",
"KE","KG","KH","KI","KM","KN","KP","KR","KW","KY","KZ","LA","LB","LC","LI","LK","LR","LS","LT",
"LU","LV","LY","MA","MC","MD","ME","MF","MG","MH","MK","ML","MM","MN","MO","MP","MQ","MR","MS",
"MT","MU","MV","MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI","NL","NO","NP","NR","NU","NZ",
"OM","PA","PE","PF","PG","PH","PK","PL","PM","PN","PR","PS","PT","PW","PY","QA","RE","RO","RS",
"RU","RW","SA","SB","SC","SD","SE","SG","SH","SI","SJ","SK","SL","SM","SN","SO","SR","SS","ST",
"SV","SX","SY","SZ","TC","TD","TF","TG","TH","TJ","TK","TL","TM","TN","TO","TR","TT","TV","TW",
"TZ","UA","UG","UM","US","UY","UZ","VA","VC","VE","VG","VI","VN","VU","WF","WS","YE","YT","ZA",
"ZM","ZW","UK" };
        public string[] taxCurrencyCode = { "SAR" };
        public string[] vatCode = { "S", "Z", "E", "O" };
        private List<string> invoices = new List<string>();
        private Dictionary<string, string> buyervat = new Dictionary<string, string>();

        public CreditNoteListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(vitaConsts.LocalizationSourceName);
        }

        public List<Dictionary<string, string>> GetInvoiceFromExcelCustom(byte[] fileBytes)
        {
            return ProcessExcelFileCustom(fileBytes, ProcessExcelRowCustom);
        }

        public Dictionary<string, string> ProcessExcelRowCustom(ISheet worksheet, int row)
        {
            var objResult = new Dictionary<string, string>();

            try
            {
                if (IsRowEmpty(worksheet, row))
                {
                    return null;
                }

                var headerRow = worksheet.GetRow(0);
                List<string> properties = new List<string>();

                for (var i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    properties.Add(headerRow.GetCell(i).StringCellValue);
                }
                var r = worksheet.GetRow(row);
                var csv = new List<string>();

                for (var i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    var cell = r.GetCell(i);
                    if (cell == null)
                        csv.Add(null);

                    else
                    {
                        var cellType = cell.CellType;

                        if (cellType == CellType.Numeric)
                        {
                            if (properties[i].Contains("Date"))
                                csv.Add(cell.DateCellValue.ToString());
                            else if (properties[i].Contains("Time"))
                            {
                                csv.Add(cell.DateCellValue.ToString().Split(' ')[1]);
                            }
                            else
                                csv.Add(cell.NumericCellValue.ToString());

                        }
                        else if (cellType == CellType.String)
                            csv.Add(cell.StringCellValue?.ToString());
                        else if (cellType == CellType.Blank)
                            csv.Add("");
                        else if (cellType == CellType.Formula)
                            csv.Add(cell.NumericCellValue.ToString());
                        else if (cellType == CellType.Boolean)
                            csv.Add(cell.BooleanCellValue.ToString());
                        else
                            csv.Add(null);
                    }

                }


                for (int i =0; i < properties.Count; i++)
                {
                    objResult.Add(properties[i], csv[i]);
                }
                objResult.Add("xml_uuid", Guid.NewGuid().ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return objResult;
        }

        //public List<CreateOrEditImportBatchDataDto> GetInvoiceFromExcel(byte[] fileBytes)
        //{
        //    return ProcessExcelFile(fileBytes, ProcessExcelRow);
        //}

        //private CreateOrEditImportBatchDataDto ProcessExcelRow(ISheet worksheet, int row)
        //{
        //    if (IsRowEmpty(worksheet, row))
        //    {
        //        return null;
        //    }

        //    var exceptionMessage = new StringBuilder();
        //    var data = new CreateOrEditImportBatchDataDto();

        //    try
        //    {
                
        //        data.InvoiceType = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(data.InvoiceType), exceptionMessage, nullCheck: true);
        //        data.IRNNo = GetOptionalValueFromRowOrNull(worksheet, row, 1, exceptionMessage, nameof(data.IRNNo), CellType.String);
        //        data.InvoiceNumber = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(data.InvoiceNumber), exceptionMessage,nullCheck:true);
        //        data.IssueDate = Convert.ToDateTime(GetRequiredDateValueFromRowOrNull(worksheet, row, 3, nameof(data.IssueDate), exceptionMessage, nullCheck: true));
        //        data.IssueTime = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(data.IssueTime), exceptionMessage, nullCheck: true);
        //        data.InvoiceCurrencyCode = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(data.InvoiceCurrencyCode), exceptionMessage, nullCheck: true);
        //        data.BillingReferenceId = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(data.BillingReferenceId), exceptionMessage, nullCheck: true);
        //        data.OrignalSupplyDate =Convert.ToDateTime(GetRequiredDateValueFromRowOrNull(worksheet, row, 7, nameof(data.OrignalSupplyDate), exceptionMessage, nullCheck: true));
        //        data.ReasonForCN = GetOptionalValueFromRowOrNull(worksheet,row,8,exceptionMessage, nameof(data.ReasonForCN), CellType.String);
        //        data.PurchaseOrderId = GetOptionalValueFromRowOrNull(worksheet,row,9,exceptionMessage, nameof(data.PurchaseOrderId), CellType.String);
        //        data.ContractId = GetOptionalValueFromRowOrNull(worksheet,row,10,exceptionMessage, nameof(data.ContractId), CellType.String);
        //        data.BuyerMasterCode = GetOptionalValueFromRowOrNull(worksheet,row,11,exceptionMessage, nameof(data.BuyerMasterCode), CellType.String);
        //        data.BuyerName = GetOptionalValueFromRowOrNull(worksheet,row,12,exceptionMessage, nameof(data.BuyerName), CellType.String, nullCheck: true);
        //        data.BuyerVatCode = GetOptionalValueFromRowOrNull(worksheet,row,13,exceptionMessage, nameof(data.BuyerVatCode), CellType.String);
        //        data.BuyerContact = GetOptionalValueFromRowOrNull(worksheet,row,14,exceptionMessage, nameof(data.BuyerContact), CellType.String);
        //        data.BuyerCountryCode = GetOptionalValueFromRowOrNull(worksheet,row,15,exceptionMessage, nameof(data.BuyerCountryCode), CellType.String);
        //        data.InvoiceLineIdentifier = GetOptionalValueFromRowOrNull(worksheet,row,16,exceptionMessage, nameof(data.InvoiceLineIdentifier), CellType.String, nullCheck: true);
        //        data.ItemMasterCode = GetOptionalValueFromRowOrNull(worksheet,row,17,exceptionMessage, nameof(data.ItemMasterCode), CellType.String);
        //        data.ItemName = GetOptionalValueFromRowOrNull(worksheet,row,18,exceptionMessage, nameof(data.ItemName), CellType.String, nullCheck: true);
        //        data.UOM = GetOptionalValueFromRowOrNull(worksheet,row,19,exceptionMessage, nameof(data.UOM), CellType.String);
        //        data.GrossPrice = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,20,exceptionMessage, nameof(data.GrossPrice), CellType.String, nullCheck: true));
        //        data.Discount = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,21,exceptionMessage, nameof(data.Discount), CellType.String));
        //        data.NetPrice = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,22,exceptionMessage, nameof(data.NetPrice), CellType.String, nullCheck: true));
        //        data.Quantity = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,23,exceptionMessage, nameof(data.Quantity), CellType.String, nullCheck: true));
        //        data.LineNetAmount = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,24,exceptionMessage, nameof(data.LineNetAmount), CellType.String, nullCheck: true));
        //        data.VatCategoryCode = GetOptionalValueFromRowOrNull(worksheet, row, 25, exceptionMessage, nameof(data.VatCategoryCode), CellType.String, nullCheck: true);
        //        data.VatRate = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet,row,26,exceptionMessage, nameof(data.VatRate), CellType.String, nullCheck: true));
        //        data.VatExemptionReasonCode = GetOptionalValueFromRowOrNull(worksheet, row, 27, exceptionMessage, nameof(data.VatExemptionReasonCode), CellType.String);
        //        data.VatExemptionReason = GetOptionalValueFromRowOrNull(worksheet, row, 28, exceptionMessage, nameof(data.VatExemptionReason), CellType.String);
        //        data.VATLineAmount = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet, row, 29, exceptionMessage, nameof(data.VATLineAmount), CellType.String, nullCheck: true));
        //        data.LineAmountInclusiveVAT = Convert.ToDecimal(GetOptionalDecimalValueFromRowOrNull(worksheet, row, 30, exceptionMessage, nameof(data.LineAmountInclusiveVAT), CellType.String, nullCheck: true));
        //        if (invoices.Contains(data.InvoiceNumber.ToUpper()))
        //        {
        //            exceptionMessage.Append("Duplicate Invoice number found;");
        //        }
        //        else
        //        {
        //            invoices.Add(data.InvoiceNumber.ToUpper());
        //        }
        //        if (buyervat.ContainsKey(data.BuyerVatCode.ToUpper()))
        //        {
        //            if (data.BuyerName.ToUpper() != buyervat.Where(a => a.Key == data.BuyerVatCode.ToUpper()).Select(p => p.Value).FirstOrDefault())
        //            {
        //                exceptionMessage.Append("Invalid buyer VAT combination;");
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(data.BuyerVatCode))
        //                buyervat.Add(data.BuyerVatCode.ToUpper(), data.BuyerName.ToUpper());
        //        }
        //        if (data.IssueDate > DateTime.Now)
        //        {
        //            exceptionMessage.Append("Issue Date can't be greater than current date;");
        //        }
        //        if (data.InvoiceCurrencyCode?.ToUpper() != "SAR")
        //        {
        //            exceptionMessage.Append("Invoice Currency code should be SAR;");

        //        }
        //        if (!string.IsNullOrWhiteSpace(data?.BuyerVatCode))
        //        {
        //            if (!data.BuyerVatCode.StartsWith("3"))
        //            {
        //                exceptionMessage.Append("Buyer VAT Code should start with 3;");
        //            }
        //            if (data.BuyerVatCode.Trim().Length != 15)
        //            {
        //                exceptionMessage.Append("Buyer VAT Code should have 15 character length;");
        //            }
        //            if (!Regex.IsMatch(data.BuyerVatCode, "\\d{15}"))
        //            {
        //                exceptionMessage.Append("Buyer VAT Code should be Numeric;");
        //            }

        //        }
        //        if (!vatCode.Contains(data.VatCategoryCode?.ToUpper()))
        //        {
        //            exceptionMessage.Append("Invalid VAT Code;");
        //        }
        //        if (!uom.Contains(data.UOM?.ToUpper()))
        //        {
        //            exceptionMessage.Append("Invalid UOM;");
        //        }
        //        if (!countryCode.Contains(data.BuyerCountryCode?.ToUpper()))
        //        {
        //            exceptionMessage.Append("Invalid Buyer Country Code;");
        //        }
        //        if (!taxCurrencyCode.Contains(data.InvoiceCurrencyCode?.ToUpper()))
        //        {
        //            exceptionMessage.Append("Invalid Invoice Currency Code;");
        //        }
        //        if (data.GrossPrice <= 0)
        //        {
        //            exceptionMessage.Append("Invalid Gross Price;");
        //        }
        //        if (decimal.Round((data.GrossPrice - (data.GrossPrice * (data.Discount / 100))),2) != decimal.Round(data.NetPrice,2))
        //        {
        //            exceptionMessage.Append("Invalid Net Price;");
        //        }
             
        //        if (data.Quantity <= 0)
        //        {
        //            exceptionMessage.Append("Invalid Quantity;");
        //        }
        //        if (decimal.Round((data.Quantity * decimal.Round((data.GrossPrice - (data.GrossPrice * (data.Discount / 100))), 2)), 2) != decimal.Round(data.LineNetAmount, 2))
        //        {
        //            exceptionMessage.Append("Invalid Line net amount;");
        //        }
        //        if (data.Discount > 100)
        //        {
        //            exceptionMessage.Append("Invalid Discount;");
        //            data.Discount = 100;
        //        }
        //        if (data.VatRate != 15)
        //        {
        //            if (data.VatRate != 0)
        //            {
        //                exceptionMessage.Append("Invalid Vat Rate;");
        //                data.VatRate = 100;
        //            }
        //        }
        //        if (data.InvoiceType.ToUpper() == "EXPORT")
        //        {

        //            if (data.BuyerCountryCode.ToUpper() == "SA")
        //            {
        //                exceptionMessage.Append("Invalid Transaction Type;");
        //            }
        //        }
        //        if (data.BuyerCountryCode.ToUpper() != "SA")
        //        {
        //            if (data.InvoiceType.ToUpper() != "EXPORT")
        //            {
        //                exceptionMessage.Append("Invalid Transaction Type;");
        //            }
        //        }
        //        if (data.VatCategoryCode.ToUpper() == "E")
        //        {
        //            if (string.IsNullOrWhiteSpace(data.VatExemptionReasonCode))
        //            {
        //                exceptionMessage.Append("Vat Exemption Reason Code is required;");
        //            }
        //            else
        //            {
        //                if (string.IsNullOrWhiteSpace(data.VatExemptionReason))
        //                {
        //                    exceptionMessage.Append("Vat Exemption Reason is required;");

        //                }
        //            }
        //        }
        //        if (decimal.Round(data.VATLineAmount, 2) != decimal.Round(((data.Quantity * decimal.Round((data.GrossPrice - (data.GrossPrice * (data.Discount / 100))), 2)) * (data.VatRate / 100)), 2))
        //        {
        //            exceptionMessage.Append("Invalid Vat Line Amount;");
        //        }
        //        decimal a = decimal.Round(data.GrossPrice - (data.GrossPrice * (data.Discount / 100)), 2);
        //        decimal b = decimal.Round(((data.Quantity * a) * (data.VatRate / 100)), 2);
        //        decimal c = decimal.Round((data.Quantity * a), 2);
        //        decimal x = decimal.Round((b + c), 2);

        //        if (decimal.Round(data.LineAmountInclusiveVAT, 2) != x)
        //        {
        //            exceptionMessage.Append("Invalid Line Amount Inclusive VAT;");
        //        }
        //        data.Error = exceptionMessage.ToString();
        //    }
        //    catch (System.Exception exception)
        //    {
        //        data.Error = exception.Message;
        //    }

        //    return data;
        //}

        private string GetRequiredValueFromRowOrNull(
             ISheet worksheet,
             int row,
             int column,
             string columnName,
             StringBuilder exceptionMessage,
             CellType? cellType = null,
              bool nullCheck = false)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                if (nullCheck)
                {
                    exceptionMessage.Append(columnName + " should not be empty;");
                }
                return String.Empty;
            }

            if (cellType.HasValue)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = "";
            if (cell?.CellType == CellType.Numeric)
                cellValue = cell.NumericCellValue.ToString();
            else
                cellValue = cell?.StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }
            if (nullCheck)
            {
                exceptionMessage.Append(columnName + " should not be empty;");
            }
            //exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string GetRequiredDateValueFromRowOrNull(
             ISheet worksheet,
             int row,
             int column,
             string columnName,
             StringBuilder exceptionMessage,
             CellType? cellType = null,
             bool nullCheck = false)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                if (nullCheck)
                {
                    exceptionMessage.Append(columnName + " is required;");
                }
                return DateTime.MinValue.ToString();
            }
            if (cellType.HasValue)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = "";
            if (cell.CellType == CellType.String)
            {
                cellValue = cell?.StringCellValue;
            }
            else
            {
                cellValue = cell?.DateCellValue.ToString();
            }
            if (cellValue == "01-01-0001 00:00:00" && nullCheck)
            {
                exceptionMessage.Append(columnName + " is required;");
            }
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            //exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }


        private string GetOptionalValueFromRowOrNull(ISheet worksheet, int row, int column, StringBuilder exceptionMessage, string columnName, CellType? cellType = null,
            bool nullCheck = false)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                if (nullCheck)
                {
                    exceptionMessage.Append(columnName + " should not be empty;");
                }
                return string.Empty;
            }

            if (cellType != null)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = "";
            if (cell.CellType == CellType.Numeric)
                cellValue = worksheet.GetRow(row).GetCell(column).NumericCellValue.ToString();

            else
                cellValue = cell.StringCellValue;

            cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            return String.Empty;
        }

        private decimal GetOptionalDecimalValueFromRowOrNull(ISheet worksheet, int row, int column, StringBuilder exceptionMessage, string columnName, CellType? cellType = null,
           bool nullCheck = false)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                if (nullCheck)
                {
                    exceptionMessage.Append(columnName + " should not be empty;");
                }
                return 0;
            }

            if (cellType != null)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = "";
            if (cell.CellType == CellType.Numeric)
                cellValue = worksheet.GetRow(row).GetCell(column).NumericCellValue.ToString();

            else
                cellValue = cell.StringCellValue;

            cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return Convert.ToDecimal(cellValue);
            }

            return 0;
        }

        private string[] GetAssignedRoleNamesFromRow(ISheet worksheet, int row, int column)
        {
            var cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ISheet worksheet, int row)
        {
            var cell = worksheet.GetRow(row)?.Cells.Count(a=>a.CellType!=CellType.Blank) == 0;
            return cell;
        }
    }
}
