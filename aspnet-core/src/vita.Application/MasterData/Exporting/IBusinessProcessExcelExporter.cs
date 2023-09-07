using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface IBusinessProcessExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessProcessForViewDto> businessProcess);
    }
}