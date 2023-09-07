using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Dto;

namespace vita.DataExporting.Excel
{
    public interface IExcelExporter
    {
        public FileDto ExportToFileVat(DataTable dt, string fileName,DateTime fromDate,DateTime toDate, string tenantName);

        public FileDto ExportToFileSalesReconciliation(DataTable dt, string fileName, DateTime fromDate, DateTime toDate, string tenantName,string header, PurchaseExcelDto input);

        public FileDto ExportToFileWht(DataTable dt, string fileName, WHTExcelDto input);
        public FileDto ExportToFilePurchase(DataTable dt, string fileName, PurchaseExcelDto input,string code, string typeName);

        public FileDto ExportToFileCreditPurchase(DataTable dt, string fileName, PurchaseExcelDto input);

        public FileDto ExportToFileDebitPurchase(DataTable dt, string fileName, PurchaseExcelDto input);

        public FileDto ExportToFileOverride(DataTable dt, string fileName, PurchaseExcelDto input, string code, string typeName);




        public FileDto ExportToFileSales(DataTable dt, string fileName, PurchaseExcelDto input,string code, string typeName);
        public FileDto ExportToMaster(DataTable dt, string fileName, PurchaseExcelDto input);

        public FileDto ExportToFileCredit(DataTable dt, string fileName, PurchaseExcelDto input,string code, string typecode);

        public FileDto ExportToFileDebit(DataTable dt, string fileName, PurchaseExcelDto input);



        public DataTable ToDataTable<T>(List<T> items, bool total);

    }
}
