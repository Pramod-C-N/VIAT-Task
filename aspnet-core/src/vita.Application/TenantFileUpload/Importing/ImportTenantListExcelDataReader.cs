using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.DataExporting.Excel.NPOI;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;


namespace vita.TenantFileUpload.Importing
{
    public class ImportTenantListExcelDataReader : NpoiExcelImporterBase<CreateOrEditImportBatchDataDto>, IImportTenantListExcelDataReader
    {
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
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return objResult;
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
