using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class AffiliationExcelExporter : NpoiExcelExporterBase, IAffiliationExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AffiliationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAffiliationForViewDto> affiliation)
        {
            return CreateExcelPackage(
                "Affiliation.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Affiliation"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, affiliation,
                        _ => _.Affiliation.Name,
                        _ => _.Affiliation.Description,
                        _ => _.Affiliation.Code,
                        _ => _.Affiliation.IsActive
                        );

                });
        }
    }
}