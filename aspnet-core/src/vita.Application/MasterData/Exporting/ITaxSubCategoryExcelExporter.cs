using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface ITaxSubCategoryExcelExporter
    {
        FileDto ExportToFile(List<GetTaxSubCategoryForViewDto> taxSubCategory);
    }
}