using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Dto;
using vita.ImportBatch.Dtos;

namespace vita.ImportBatch.Exporting
{
    public interface ISalesFileExcelExporter:IApplicationService
    {
        FileDto ExportToFile(List<ImportBatchDataDto> paymentDetails);
        FileDto ExportToFile(DataTable dt, string fileName);

        FileDto ExportToFileWithCustomHeader(DataTable dt, string fileName,string header);


    }
}
