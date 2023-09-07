using System;
using System.Collections.Generic;
using System.IO;
using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using vita.Dto;
using vita.Storage;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using NPOI.SS.Util;
using static vita.Configuration.AppSettings.UiManagement;
using MailKit;
using iText.Layout.Element;
using NPOI.SS.Formula.Functions;

namespace vita.DataExporting.Excel.NPOI
{
    public abstract class NpoiExcelExporterBase : vitaServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private IWorkbook _workbook;

        private readonly Dictionary<string, ICellStyle> _dateCellStyles = new();
        private readonly Dictionary<string, IDataFormat> _dateDateDataFormats = new();

        private ICellStyle GetDateCellStyle(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateCellStyles.Clear();
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateCellStyles.ContainsKey(dateFormat))
            {
                return _dateCellStyles.GetValueOrDefault(dateFormat);
            }

            var cellStyle = cell.Sheet.Workbook.CreateCellStyle();
            _dateCellStyles.Add(dateFormat, cellStyle);
            return cellStyle;
        }

        private IDataFormat GetDateDataFormat(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateDateDataFormats.ContainsKey(dateFormat))
            {
                return _dateDateDataFormats.GetValueOrDefault(dateFormat);
            }

            var dataFormat = cell.Sheet.Workbook.CreateDataFormat();
            _dateDateDataFormats.Add(dateFormat, dataFormat);
            return dataFormat;
        }

        protected NpoiExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<XSSFWorkbook> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var workbook = new XSSFWorkbook();

            creator(workbook);

            Save(workbook, file);

            return file;
        }

        protected void AddHeader(ISheet sheet, string header, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            sheet.CreateRow(0);
            AddHeaderCustomReport(sheet, 0, 0, header,"Black");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, headerTexts.Length-1));

            var cellStyle = sheet.GetRow(0).GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);


            sheet.CreateRow(1);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i, headerTexts[i],1);
            }
        }

        protected void AddHeader(ISheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            

            sheet.CreateRow(0);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i, headerTexts[i]);
            }
        }

       
        protected void AddHeader(ISheet sheet, int columnIndex, string headerText,int rowIndex=0)
        {

            var cell = sheet.GetRow(rowIndex).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;
        }

        protected void AddObjects<T>(ISheet sheet, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(i);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var value = propertySelectors[j](items[i - 1]);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }
                }
            }
        }

        protected void AddObjectsFromDatatable(ISheet sheet, DataTable dt,int skipRows=1)
        {

            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + skipRows);

                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            cell.SetCellValue(value.ToString());
                        }
                    }
                }
               
                for (var i = 0; i < dt.Columns.Count; i++)
                    sheet.AutoSizeColumn(i);
            }
            

            catch (Exception e)
            {
                throw e;
            }
        }

        protected virtual void Save(XSSFWorkbook excelPackage, FileDto file)
        {
            using (var stream = new MemoryStream())
            {
                excelPackage.Write(stream);
                _tempFileCacheManager.SetFile(file.FileToken, stream.ToArray());
            }
        }

        protected void SetCellDataFormat(ICell cell, string dataFormat)
        {
            if (cell == null)
                return;

            var dateStyle = GetDateCellStyle(cell, dataFormat);
            var format = GetDateDataFormat(cell, dataFormat);

            dateStyle.DataFormat = format.GetFormat(dataFormat);
            cell.CellStyle = dateStyle;
            if (DateTime.TryParse(cell.StringCellValue, out var datetime))
                cell.SetCellValue(datetime);
        }

        protected void AddHeaderCustomReport(ISheet sheet, int columnIndex, int rowIndex, string headerText, string color, bool isBold = false)
        {
            var row = sheet.GetRow(rowIndex);
            if(row == null)
                row = sheet.CreateRow(rowIndex);
            var cell = row.CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            cellStyle.WrapText = true;
            cell.Row.Height = 512;
            if (isBold)
            {
                var font = sheet.Workbook.CreateFont();
                font.IsBold = true;
                font.FontHeightInPoints = 12;
                cellStyle.SetFont(font);
            }
            if (color == "yellow")
            {
                //set background color to yellow
                cellStyle.FillForegroundColor = IndexedColors.Yellow.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            else if (color == "green")
            {
                //set background color to green
                cellStyle.FillForegroundColor = IndexedColors.Green.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            else if (color == "red")
            {
                //set background color to red
                cellStyle.FillForegroundColor = IndexedColors.Red.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            cell.CellStyle = cellStyle;
        }
        protected void AddHeaderFooterReport(ISheet sheet, int columnIndex, int rowIndex, string headerText, string color, bool isBold = false)
        {
            var cell = sheet.GetRow(rowIndex).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            cellStyle.WrapText = true;
            if (isBold)
            {
                var font = sheet.Workbook.CreateFont();
                font.IsBold = true;
                font.FontHeightInPoints = 12;
                cellStyle.SetFont(font);
            }
            if (color == "yellow")
            {
                //set background color to yellow
                cellStyle.FillForegroundColor = IndexedColors.Yellow.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            else if (color == "green")
            {
                //set background color to green
                cellStyle.FillForegroundColor = IndexedColors.Green.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            else if (color == "red")
            {
                //set background color to red
                cellStyle.FillForegroundColor = IndexedColors.Red.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
            cell.CellStyle = cellStyle;
        }
        protected void AddHeaderVat(ISheet sheet, DateTime fromDate, DateTime toDate, string tenantName, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            var rowNum = 0;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 1, rowNum, "Tax payee name:", null, true);
            AddHeaderCustomReport(sheet, 1 + 1, rowNum, tenantName, null);

            rowNum++;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 1, rowNum, "Tax return from:", null, true);
            AddHeaderCustomReport(sheet, 1 + 1, rowNum, fromDate.ToString("dd/MM/yyyy") + " to " + toDate.ToString("dd/MM/yyyy"), null);

            rowNum++;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 1, rowNum, "Do you have sales or purchases subject to 5% in accordance with the transitional provisions, or sales, purchases, imports or amendments subject to 5% and would you like to acknowledge them during this period?", "yellow");
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 1, 1 + 3));
            AddHeaderCustomReport(sheet, 1 + 4, rowNum, "Yes", "yellow");
            AddHeaderCustomReport(sheet, 1 + 5, rowNum, "No", "yellow");

            rowNum++;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 1, rowNum, "Do you have supplies that are subject to tax at the basic rate for government agencies according to the contracting and government procurement?", "yellow");
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 1, 1 + 3));
            AddHeaderCustomReport(sheet, 1 + 4, rowNum, "Yes", "yellow");
            AddHeaderCustomReport(sheet, 1 + 5, rowNum, "No", "yellow");

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);
            for (var i = 1; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i + 3, rowNum, headerTexts[i], null, true);
            }

            rowNum++;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 1, rowNum, "Value added tax on sales:", null, true);
        }

        protected void AddObjectsFromDatatableVat(ISheet sheet, DataTable dt)
        {
            try
            {
                //var breakPt = 0;
                for (var i = 0; i <=dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 7);
                    if (i == 8)
                    {
                        //breakPt = i+1;
                       // break; //hiding purchase
                        AddHeaderCustomReport(sheet, 1, i + 7, "Value added tax on purchases:", null, true);
                        continue;
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {

                        var r = i;
                        if (i < 8)
                        {
                            r = i;
                        }
                        else
                        {
                            r = i - 1;
                        }
                        if (j == 0)
                        {
                            var cell = row.CreateCell(j + 2);
                            var value = dt.Rows[r][j];
                            if (value != null)
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }
                        else
                        {
                            var cell = row.CreateCell(j + 3);
                            var value = dt.Rows[r][j];
                            if (value != null)
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }

                    }
                }
                var footerR = dt.Rows.Count + 8;

                sheet.CreateRow(footerR);

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "Recovery information:", null, true);

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "The information on your tax return form indicates that there is a balance in your account, and your balance will be carried forward to the next tax period. If you want to request for a refund, please click here:", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 3));
                AddHeaderCustomReport(sheet, 1 + 4, footerR, "Yes", "yellow");
                AddHeaderCustomReport(sheet, 1 + 5, footerR, "No", "yellow");

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "Terms and conditions:", null, true);

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "1. It is aasumed that the taxpayer has read and understood the KSA VAT rules and regulations and that the information provided to the taxpayer's knowledge is accurate, correct and complete.", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 5));

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "2. The Authority reserves the right to request and obtain information or to request financial or administrative records of the taxpayer and their facilities for comparison and documentation the information contained in this statement.", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 5));

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "3. The Authority reserves the right to open a review case to verify the validity of this declaration form and for any previous declarations for a maximum of 5 years which may result in imposing fines according to the Value-added tax rules and regulations in the Kingdom.", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 5));

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "4. After submitting this Form, and if the taxpayers discover that they need to make an an=djustment, the voluntary disclosure of the data must be made if an amount of modification is more or less than SAR5,000", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 5));

                footerR++;
                sheet.CreateRow(footerR);

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "Certification Form:", null, true);

                footerR++;
                sheet.CreateRow(footerR);
                AddHeaderCustomReport(sheet, 1, footerR, "I certify that the information provided in this affidavit is to the best of my knowledge accurate, true and complete in all respects and that I am the person authorized to fill out this to confirm or auhtorized to sign on behalf of that person, and I am also aware of the heavy penalties for the inclusion of false information.", "yellow");
                sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 5));

                //auto size all columns of the sheet
                for (var i = 0; i < 10; i++)
                {
                    sheet.AutoSizeColumn(i);
                }




            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void AddHeaderWht(ISheet sheet, WHTExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 7));

            rowNum++;
            var r = sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, "MONTHLY WITHHOLDING TAX FORM", null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 7));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            if (input.Month == input.ToMonth)
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 7));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, "Withholder's Name: " + input.WithholderName + "        Fiscal Year: " + input.FiscalYear + "         Financial Number: " + input.FinancialNumber, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 7));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }


        protected void AddHeaderSalesReconciliation(ISheet sheet, DateTime fromdate, DateTime todate, string header, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;

                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);

                excol = 5;


            
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT + " , " + " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
             r = sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, header, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 5));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            if (fromdate.Month == todate.Month)
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + fromdate.ToString("MMM") + "     Year: " + fromdate.Year, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + fromdate + "     To Date: " + todate, null, true);

            }

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 5));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            // rowNum++;
            // r = sheet.CreateRow(rowNum);
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Withholder's Name: " + input.WithholderName + "        Fiscal Year: " + input.FiscalYear + "         Financial Number: " + input.FinancialNumber, null, true);
            // sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 7));
            // cellStyle = r.GetCell(0).CellStyle;
            // cellStyle.Alignment = HorizontalAlignment.Center;
            // cellStyle.VerticalAlignment = VerticalAlignment.Center;
            // cell = r.GetCell(0);
            // cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }

        protected void AddObjectsFromDatatableSalesReconciliation(ISheet sheet, DataTable dt)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 6);
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        

                        //var r = i;
                        //if (i < 8)
                        //{
                        //    r = i;
                        //}
                        //else
                        //{
                        //    r = i ;
                        //}
                        if (j == 0)
                        {
                            var cell = row.CreateCell(j);
                           
                                var value = dt.Rows[i][j];
                            
 
                            if (value != null)
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }
                        else { 
                            var cell = row.CreateCell(j);
                            var value = dt.Rows[i][j];
                            if (value != null)
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }

                    }
                }
                
                //auto size all columns of the sheet
                for (var i = 0; i < 10; i++)
                {
                    sheet.AutoSizeColumn(i);
                }




            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void AddHeaderSales(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum,  input.Name  , null, true);
                //AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
                excol = 11;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum,  input.Name  , null, true);
                // AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
                excol = 8;


            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT +" , "+ " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;



            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 12;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 8;


            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
            //    excol = 12;
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
            //    excol = 8;


            //}
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Customer's Name: " + input.Name + "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }

        protected void AddHeaderMaster(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;
            if (input.Type == "VENDOR")
            {
                //AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                excol = 14;
            }
            else if(input.Type == "CUSTOMER")
            {
               // AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                 AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                excol = 14;


            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                excol = 14;

            }

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT + " , " + " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;



            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "VENDOR")
            {
                //AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                AddHeaderCustomReport(sheet, 0, rowNum, "Vendor Report", null, true);
            }
            else if (input.Type == "CUSTOMER")
            {
                // AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                AddHeaderCustomReport(sheet, 0, rowNum, "Customer Report", null, true);


            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Tenant Report", null, true);

            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);


            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
            //    excol = 12;
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
            //    excol = 8;


            //}
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Customer's Name: " + input.Name + "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }



        protected void AddHeaderOverride(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;

                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                excol = 7;
 
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT + " , " + " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;



            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);

                AddHeaderCustomReport(sheet, 0, rowNum, "Override Option Exercised Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 7;
      
  
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
            //    excol = 12;
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
            //    excol = 8;


            //}
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Customer's Name: " + input.Name + "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }



        protected void AddHeaderCredit(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                //AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
                excol = 12;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                // AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
                excol = 8;


            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT + " , " + " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;



            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Credit(Sales) Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 12;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Credit(Sales) Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 8;


            }

           // AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note(Sales) Detailed Report", null, true);
            //    excol = 12;
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note(Sales) Daywise Report", null, true);
            //    excol = 8;


            //}
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Customer's Name: " + input.Name + "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }


        protected void AddHeaderDebit(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            int excol = 0;
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                //AddHeaderCustomReport(sheet, 0, rowNum, "Sales Detailed Report", null, true);
                excol = 12;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
                // AddHeaderCustomReport(sheet, 0, rowNum, "Sales Daywise Report", null, true);
                excol = 8;


            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "  VAT ID: " + input.VAT + " , " + " ADDRESS: " + input.Address, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;



            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note(Sales) Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 12;
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note(Sales) Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
                excol = 8;


            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);

            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note(Sales) Detailed Report", null, true);
            //    excol = 12;
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note(Sales) Daywise Report", null, true);
            //    excol = 8;


            //}
            //// AddHeaderCustomReport(sheet, 0, rowNum, "Customer's Name: " + input.Name + "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + excol));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }


        protected void AddHeaderPurchase(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


                //rowNum++;
                var r = sheet.CreateRow(rowNum);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name , null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);

            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
                var cellStyle = r.GetCell(0).CellStyle;
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                var cell = r.GetCell(0);
                cell.CellStyle = cellStyle;
            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;


            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Purchase Entry Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
               
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Purchase Entry Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);


            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Purchase Entry Detailed Report", null, true);
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum,"Purchase Entry Daywise Report", null, true);

            //}
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }


        protected void AddHeaderCreditPurchase(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);

            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;
            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;


            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note (Purchase) Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note (Purchase) Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);


            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note (Purchase) Detailed Report", null, true);
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Credit Note (Purchase) Daywise Report", null, true);

            //}
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }


        protected void AddHeaderDebitPurchase(ISheet sheet, PurchaseExcelDto input, params string[] headerTexts)
        {

            var rowNum = 0;
            //sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Zakat, Tax and Customs Authority", null, true);
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));


            //rowNum++;
            var r = sheet.CreateRow(rowNum);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, input.Name, null, true);

            }
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            var cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var cell = r.GetCell(0);
            cell.CellStyle = cellStyle;
            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            AddHeaderCustomReport(sheet, 0, rowNum, "        VAT: " + input.VAT + "         ADDRESS: " + input.Address, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;


            rowNum++;
            r = sheet.CreateRow(rowNum);
            //AddHeaderCustomReport(sheet, 0, rowNum, "Month: " + input.Month + "     Year: " + input.Year, null, true);
            if (input.Type == "Detailed")
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note (Purchase) Detailed Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);
            }
            else
            {
                AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note (Purchase) Daywise Report for the period from :" + input.FromDate + " To: " + input.ToDate, null, true);


            }
            //AddHeaderCustomReport(sheet, 0, rowNum, "From Date: " + input.FromDate + "     To Date: " + input.ToDate, null, true);
            sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            cellStyle = r.GetCell(0).CellStyle;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cell = r.GetCell(0);
            cell.CellStyle = cellStyle;

            //rowNum++;
            //r = sheet.CreateRow(rowNum);
            //if (input.Type == "Detailed")
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note (Purchase) Detailed Report", null, true);
            //}
            //else
            //{
            //    AddHeaderCustomReport(sheet, 0, rowNum, "Debit Note (Purchase) Daywise Report", null, true);

            //}
            //sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, 0 + 16));
            //cellStyle = r.GetCell(0).CellStyle;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cell = r.GetCell(0);
            //cell.CellStyle = cellStyle;

            rowNum++;
            sheet.CreateRow(rowNum);
            rowNum++;
            sheet.CreateRow(rowNum);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderCustomReport(sheet, i, rowNum, headerTexts[i], null, true);
            }


        }



        protected void AddObjectsFromDatatableWht(ISheet sheet, DataTable dt, WHTExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 6);
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                        c.SetCellValue("Total");


                    }
                }



                if (input.Type == "Return")
                {
                    var footerR = dt.Rows.Count + 8;

                    sheet.CreateRow(footerR);

                    footerR++;
                    sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                    footerR++;
                    sheet.CreateRow(footerR);

                    footerR++;
                    var r = sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "Declaration", null, true);
                    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                    var cellStyle = r.GetCell(0).CellStyle;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    var cell = r.GetCell(0);
                    cell.CellStyle = cellStyle;

                    footerR++;
                    r = sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "I the undersigned hereby certify that all amounts reported in this return are correct and reflect the amount due for tax withholding during the month, and are calculated in accordance with Article 68 of the Income Tax Regulations in the Kingdom of Saudi Arabia.", null);
                    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR + 2, 0, 0 + 7));
                    cellStyle = r.GetCell(0).CellStyle;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell = r.GetCell(0);
                    cell.CellStyle = cellStyle;

                    footerR++;
                    footerR++;

                    footerR++;
                    r = sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "Name", null);
                    AddHeaderCustomReport(sheet, 2, footerR, "Signature", null);
                    AddHeaderCustomReport(sheet, 4, footerR, "Date", null);
                    AddHeaderCustomReport(sheet, 6, footerR, "Stamp", null);

                    footerR++;
                    r = sheet.CreateRow(footerR);
                    footerR++;
                    r = sheet.CreateRow(footerR);
                    footerR++;
                    r = sheet.CreateRow(footerR);
                    footerR++;
                    r = sheet.CreateRow(footerR);

                    footerR++;
                    r = sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "1)", null);
                    AddHeaderCustomReport(sheet, 1, footerR, "This form should bs submitted to ZATCA within the first 10 days of the month following the month during which the payment was made to the payee", null);
                    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 6));


                    footerR++;
                    r = sheet.CreateRow(footerR);
                    AddHeaderCustomReport(sheet, 0, footerR, "2)", null);
                    AddHeaderCustomReport(sheet, 1, footerR, "If the party receiving any of the above payments exceeds one payee, a detailed statement should be enclosed showing the name of each payee, date and amount of payments", null);
                    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 1, 1 + 6));

                }





                //auto size all columns of the sheet
                for (var i = 0; i < 10; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void AddObjectsFromDatatablePurchase(ISheet sheet, DataTable dt, PurchaseExcelDto input,string code)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                    if(i== dt.Rows.Count - 1)
                    {

                        //sheet.CreateRow(dt.Rows.Count + 3);
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                            c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            if(code=="VATPUR000")
                            {
                                c = row.GetCell(6);
                                c.SetCellValue(" ");
                                c = row.GetCell(7);
                                c.SetCellValue(" ");
                                c = row.GetCell(11);
                                c.SetCellValue(" ");
                                c = row.GetCell(12);
                                c.SetCellValue(" ");
                            }
                            else
                            {
                                c = row.GetCell(7);
                                c.SetCellValue(" ");
                                c = row.GetCell(8);
                                c.SetCellValue(" ");
                                c = row.GetCell(12);
                                c.SetCellValue(" ");
                                c = row.GetCell(13);
                                c.SetCellValue(" ");
                            }

                           
                        }
                        else
                        {
                            //c = row.GetCell(1);
                            //c.SetCellValue(" ");
                            c = row.GetCell(7);
                            c.SetCellValue(" ");
                            c = row.GetCell(6);
                            c.SetCellValue(" ");
                            c = row.GetCell(11);
                            c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);

                 




                //auto size all columns of the sheet
                for (var i = 0; i < 17; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableSales(ISheet sheet, DataTable dt, PurchaseExcelDto input,string code)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {

                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }


                    if (i == dt.Rows.Count - 1)
                    {
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);

                        c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            if(code == "VATSAL000")
                            {
                                c = row.GetCell(9);
                            }
                            else
                            {
                                c = row.GetCell(9);
                            }
                            
                            c.SetCellValue(" ");
                        }
                        else
                        {
                           // c = row.GetCell(1);
                           // c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 12; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableMaster(ISheet sheet, DataTable dt, PurchaseExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {

                    var row = sheet.CreateRow(i + 5);
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }
}

                




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 14; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableOverride(ISheet sheet, DataTable dt, PurchaseExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                 

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 10; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableCredit(ISheet sheet, DataTable dt, PurchaseExcelDto input,string code)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                        c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            if(code==string.Empty || code== "VATCNS000")
                            {
                                c = row.GetCell(10);
                            }
                            else
                            {
                                c = row.GetCell(11);
                            }
                            
                            c.SetCellValue(" ");
                        }
                        else
                        {
                            //c = row.GetCell(1);
                           // c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 12; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }



        protected void AddObjectsFromDatatableDebit(ISheet sheet, DataTable dt, PurchaseExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                        c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            c = row.GetCell(10);
                            c.SetCellValue(" ");
                        }
                        else
                        {
                            //c = row.GetCell(1);
                            //c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 13; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableCreditPurchase(ISheet sheet, DataTable dt, PurchaseExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        //sheet.CreateRow(dt.Rows.Count + 3);
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                        c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            c = row.GetCell(9);
                            c.SetCellValue(" ");

                            c = row.GetCell(10);
                            c.SetCellValue(" ");


                            c = row.GetCell(11);
                            c.SetCellValue(" ");

                            c = row.GetCell(15);
                            c.SetCellValue(" ");
                        }
                        else
                        {
                            //c = row.GetCell(1);
                            //c.SetCellValue(" ");

                            c = row.GetCell(7);
                            c.SetCellValue(" ");


                            c = row.GetCell(8);
                            c.SetCellValue(" ");

                            c = row.GetCell(9);
                            c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 18; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }


        protected void AddObjectsFromDatatableDebitPurchase(ISheet sheet, DataTable dt, PurchaseExcelDto input)
        {
            try
            {

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 5);
                    if (i == dt.Rows.Count - 1)
                    {
                        row = sheet.CreateRow(i + 6);
                    }
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var rInd = i + 7;
                        var c = row.CreateCell(j);
                        var value = dt.Rows[i][j];
                        if (value != null)
                        {
                            c.SetCellValue(value.ToString());
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        var font = sheet.Workbook.CreateFont();
                        font.IsBold = true;
                        font.FontHeightInPoints = 12;
                        var value = dt.Rows[i][0];
                        var c = row.GetCell(0);
                        c.SetCellValue("Total");
                        c.CellStyle = sheet.Workbook.CreateCellStyle();
                        c.CellStyle.SetFont(font);
                        for (var k = 0; k < dt.Columns.Count; k++)
                        {
                            c = row.GetCell(k);
                            c.CellStyle = sheet.Workbook.CreateCellStyle();
                            c.CellStyle.SetFont(font);
                        }
                        if (input.Type == "Detailed")
                        {
                            c = row.GetCell(9);
                            c.SetCellValue(" ");

                            c = row.GetCell(10);
                            c.SetCellValue(" ");


                            c = row.GetCell(11);
                            c.SetCellValue(" ");

                            c = row.GetCell(15);
                            c.SetCellValue(" ");
                        }
                        else
                        {
                            //c = row.GetCell(1);
                            //c.SetCellValue(" ");

                            c = row.GetCell(7);
                            c.SetCellValue(" ");


                            c = row.GetCell(8);
                            c.SetCellValue(" ");

                            c = row.GetCell(9);
                            c.SetCellValue(" ");
                        }


                    }

                }




                //var footerR = dt.Rows.Count + 8;

                //    sheet.CreateRow(footerR);

                //    footerR++;
                //    sheet.CreateRow(footerR);
                //    AddHeaderCustomReport(sheet, 0, footerR, "This return is prepared in Arabic and English. In case of inconsistency, the Arabic version should prevail.", null, false);
                //    sheet.AddMergedRegion(new CellRangeAddress(footerR, footerR, 0, 0 + 7));
                //    footerR++;
                //    sheet.CreateRow(footerR);






                //auto size all columns of the sheet
                for (var i = 0; i < 18; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 512);
                }




            }

            catch (Exception e)
            {
                throw e;
            }
        }





    }
}
