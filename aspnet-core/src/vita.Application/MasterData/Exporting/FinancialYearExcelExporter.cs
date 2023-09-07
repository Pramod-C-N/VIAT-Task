using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class FinancialYearExcelExporter : NpoiExcelExporterBase, IFinancialYearExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FinancialYearExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFinancialYearForViewDto> financialYear)
        {
            return CreateExcelPackage(
                "FinancialYear.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FinancialYear"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("EffectiveFromDate"),
                        L("EffectiveTillEndDate"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, financialYear,
                        _ => _.FinancialYear.Name,
                        _ => _.FinancialYear.Description,
                        _ => _.FinancialYear.Code,
                        _ => _.FinancialYear.EffectiveFromDate,
                        _ => _.FinancialYear.EffectiveTillEndDate,
                        _ => _.FinancialYear.IsActive
                        );

                });
        }
    }
}