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
using Abp.Localization.Sources;
using Abp.Localization;
using System.Linq;
using System.Text;
using System;
using vita.CreditNoteFileUpload.Importing;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;
using System.IO;

namespace vita.DebitNoteSalesFileUpload.Importing
{
    internal class DebitNoteSalesListExcelDataReader : NpoiExcelImporterBase<CreateOrEditImportBatchDataDto>, IDebitNoteSalesListExcelDataReader
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

        public DebitNoteSalesListExcelDataReader(ILocalizationManager localizationManager)
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

                for (var i = r.FirstCellNum; i < r.LastCellNum; i++)
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


                for (int i = 0; i < properties.Count; i++)
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

        private IEnumerable<string[]> ReadSV(StreamReader reader, params string[] separators)
        {
            var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(reader);
            parser.SetDelimiters(separators);
            parser.HasFieldsEnclosedInQuotes = true;
            while (!parser.EndOfData)
                yield return parser.ReadFields();
        }
        public byte[] ConvertCsvToExcel(byte[] csvBytes)
        {
            using (MemoryStream csvStream = new MemoryStream(csvBytes))
            {
                // Read CSV using StreamReader
                using (StreamReader csvReader = new StreamReader(csvStream))
                {
                    var li = ReadSV(csvReader, new string[] { "," });
                    // Create a new Excel workbook
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet("Sheet1");
                    int rowIndex = 0;
                    foreach (var csvRow in li)
                    {
                        // Create a new Excel row
                        IRow row = sheet.CreateRow(rowIndex);

                        for (int colIndex = 0; colIndex < csvRow.Length; colIndex++)
                        {
                            // Set cell values for each column in the row
                            row.CreateCell(colIndex).SetCellValue(csvRow[colIndex]);
                        }

                        rowIndex++;
                    }


                    // Convert Excel data to byte array
                    using (MemoryStream excelStream = new MemoryStream())
                    {
                        workbook.Write(excelStream);
                        return excelStream.ToArray();
                    }
                }
            }
        }


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
            //var cell = worksheet.GetRow(row)?.Cells.FirstOrDefault();
            //return cell == null || string.IsNullOrWhiteSpace(cell.StringCellValue);
            var cell = worksheet.GetRow(row)?.Cells.Count(a => a.CellType != CellType.Blank) == 0;
            return cell;
        }
    }
}
