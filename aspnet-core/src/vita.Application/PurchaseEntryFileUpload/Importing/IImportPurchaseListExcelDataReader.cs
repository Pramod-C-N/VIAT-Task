using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using vita.CreditNoteFileUpload.Dtos;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;

namespace vita.PurchaseFileUpload.Importing
{
    public interface IImportPurchaseListExcelDataReader : ITransientDependency
    {
        //List<CreateOrEditImportBatchDataDto> GetInvoiceFromExcel(byte[] fileBytes);
        List<Dictionary<string, string>> GetInvoiceFromExcelCustom(byte[] fileBytes);
        byte[] ConvertCsvToExcel(byte[] csvBytes);


    }
}
