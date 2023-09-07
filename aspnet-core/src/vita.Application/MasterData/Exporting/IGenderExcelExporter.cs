using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface IGenderExcelExporter
    {
        FileDto ExportToFile(List<GetGenderForViewDto> gender);
    }
}