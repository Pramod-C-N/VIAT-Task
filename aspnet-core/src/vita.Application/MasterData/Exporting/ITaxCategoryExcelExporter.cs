using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface ITaxCategoryExcelExporter
    {
        FileDto ExportToFile(List<GetTaxCategoryForViewDto> taxCategory);
    }
}