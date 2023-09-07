using System.Collections.Generic;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData.Exporting
{
    public interface IFinancialYearExcelExporter
    {
        FileDto ExportToFile(List<GetFinancialYearForViewDto> financialYear);
    }
}