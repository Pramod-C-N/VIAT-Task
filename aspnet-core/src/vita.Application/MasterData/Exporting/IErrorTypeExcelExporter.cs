using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface IErrorTypeExcelExporter
    {
        FileDto ExportToFile(List<GetErrorTypeForViewDto> errorType);
    }
}