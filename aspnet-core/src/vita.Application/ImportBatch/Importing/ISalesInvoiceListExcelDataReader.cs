using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.ImportBatch.Dtos;

namespace vita.ImportBatch.Importing
{
    public interface ISalesInvoiceListExcelDataReader : ITransientDependency
    {
        //List<CreateOrEditImportBatchDataDto> GetInvoiceFromExcel(byte[] fileBytes);
        List<Dictionary<string, string>> GetInvoiceFromExcelCustom(byte[] fileBytes);
        byte[] ConvertCsvToExcel(byte[] csvBytes);

    }
}
