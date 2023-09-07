using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using vita.CreditNoteFileUpload.Dtos;
using vita.ImportBatch.Dtos;
//using vita.StandardFileUpload.Dtos;

namespace vita.DebitNoteSalesFileUpload.Importing
{
    public interface IDebitNoteSalesListExcelDataReader : ITransientDependency
    {
        List<Dictionary<string, string>> GetInvoiceFromExcelCustom(byte[] fileBytes);
    }
}