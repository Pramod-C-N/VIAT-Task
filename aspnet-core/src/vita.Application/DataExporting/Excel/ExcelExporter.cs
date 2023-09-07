using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users.Dto;
using vita.DataExporting.Excel;
using vita.DataExporting.Excel.NPOI;
using vita.Dto;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Exporting;
using vita.ImportBatch.Importing;
using vita.Storage;

namespace vita.ImportBatch.Exporting
{
    public class ExcelExporter : NpoiExcelExporterBase, IExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFileVat(DataTable dt, string fileName, DateTime fromDate, DateTime toDate, string tenantName)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderVat(sheet, fromDate, toDate, tenantName, row);

                       AddObjectsFromDatatableVat(sheet, dt);
                   });
        }

        public FileDto ExportToFileSalesReconciliation(DataTable dt, string fileName, DateTime fromDate, DateTime toDate, string tenantName,string Header, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderSalesReconciliation(sheet, fromDate, toDate,Header,input,row);

                       AddObjectsFromDatatableSalesReconciliation(sheet, dt);
                   });
        }

        public FileDto ExportToFileWht(DataTable dt, string fileName, WHTExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderWht(sheet, input, row);

                       AddObjectsFromDatatableWht(sheet, dt, input);
                   });
        }
        public FileDto ExportToFilePurchase(DataTable dt, string fileName, PurchaseExcelDto input,string code)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderPurchase(sheet, input, row);

                       AddObjectsFromDatatablePurchase(sheet, dt, input,code);
                   });
        }

        public FileDto ExportToFileCreditPurchase(DataTable dt, string fileName, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderCreditPurchase(sheet, input, row);

                       AddObjectsFromDatatableCreditPurchase(sheet, dt, input);
                   });
        }

        public FileDto ExportToFileDebitPurchase(DataTable dt, string fileName, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderDebitPurchase(sheet, input, row);

                       AddObjectsFromDatatableDebitPurchase(sheet, dt, input);
                   });
        }
        public FileDto ExportToFileSales(DataTable dt, string fileName, PurchaseExcelDto input, string code)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderSales(sheet, input, row);

                       AddObjectsFromDatatableSales(sheet, dt, input, code);
                   });
        }
        public FileDto ExportToMaster(DataTable dt, string fileName, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

            var sheet = excelPackage.CreateSheet("Sheet1");
            string[] row = new string[dt.Columns.Count];
            for (var i = 0; i < dt.Columns.Count; i++)
                row[i] = dt.Columns[i].ColumnName;

                       AddHeaderMaster(sheet, input, row);

            AddObjectsFromDatatableMaster(sheet, dt, input);
        });
        }
    public FileDto ExportToFileOverride(DataTable dt, string fileName, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderOverride(sheet, input, row);

                       AddObjectsFromDatatableOverride(sheet, dt, input);
                   });
        }
        public FileDto ExportToFileCredit(DataTable dt, string fileName, PurchaseExcelDto input,string code)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderCredit(sheet, input, row);

                       AddObjectsFromDatatableCredit(sheet, dt, input,code);
                   });
        }
        public DataTable ToDataTable<T>(List<T> items, bool total)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int f = 0;
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                if (f == 0)
                    dataTable.Columns.Add(prop.Name);
                else
                    dataTable.Columns.Add(prop.Name, type);
              //  f++;
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            //-----------------adding footer --------------------------------------
            try
            {
                if (total)
                {
                    decimal[] RowTotals = new decimal[dataTable.Columns.Count];
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        var dtype = dataTable.Columns[i].DataType;
                        if (dtype == typeof(Int16) || dtype == typeof(Int32) || dtype == typeof(Int64) || dtype == typeof(Double) || dtype == typeof(decimal))
                        {
                            RowTotals[i] = 0;

                        }
                        else
                        {
                            RowTotals[i] = -1;

                        }

                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //loop all the columns in the row
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            //add the values for each cell to the total
                            if (RowTotals[i] != -1)
                                RowTotals[i] += Convert.ToDecimal(row[dataTable.Columns[i].ColumnName]);
                        }
                    }
                    DataRow dr = dataTable.NewRow();
                    dr[0] = "Total";

                    for (int i = 1; i < dataTable.Columns.Count; i++)
                    {
                        if (RowTotals[i] != -1)
                            dr[i] = RowTotals[i];
                        else
                            dr[i] = null;
                    }

                    dataTable.Rows.Add(dr);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return dataTable;
        }
        public FileDto ExportToFileDebit(DataTable dt, string fileName, PurchaseExcelDto input)
        {
            return CreateExcelPackage(
                   fileName + ".xlsx",
                   excelPackage =>
                   {

                       var sheet = excelPackage.CreateSheet("Sheet1");
                       string[] row = new string[dt.Columns.Count];
                       for (var i = 0; i < dt.Columns.Count; i++)
                           row[i] = dt.Columns[i].ColumnName;

                       AddHeaderDebit(sheet, input, row);

                       AddObjectsFromDatatableDebit(sheet, dt, input);
                   });
        }

    }



}
