using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.ImportBatch.Dtos;

namespace vita.ImportPaymentFileupload.Importing
{
    public interface IImportPaymentListExcelDataReader : ITransientDependency
    {

        List<Dictionary<string, string>> GetInvoiceFromExcelCustom(byte[] fileBytes);

    }
}
