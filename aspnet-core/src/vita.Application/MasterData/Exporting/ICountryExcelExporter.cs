using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface ICountryExcelExporter
    {
        FileDto ExportToFile(List<GetCountryForViewDto> country);
    }
}