using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users.Importing.Dto;
using vita.Dto;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Dtos;

namespace vita.ImportBatch.Importing
{
    public interface IInvalidInvoiceExporter
    {
        FileDto ExportToFile(List<CreateOrEditImportBatchDataDto> userListDtos);
    }
}
