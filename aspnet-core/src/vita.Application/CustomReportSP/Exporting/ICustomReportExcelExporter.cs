using System.Collections.Generic;
using vita.CustomReportSP.Dtos;
using vita.Dto;

namespace vita.CustomReportSP.Exporting
{
    public interface ICustomReportExcelExporter
    {
        FileDto ExportToFile(List<GetCustomReportForViewDto> customReport);
    }
}