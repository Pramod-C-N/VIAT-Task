using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class OrganisationTypeExcelExporter : NpoiExcelExporterBase, IOrganisationTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrganisationTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrganisationTypeForViewDto> organisationType)
        {
            return CreateExcelPackage(
                "OrganisationType.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrganisationType"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, organisationType,
                        _ => _.OrganisationType.Name,
                        _ => _.OrganisationType.Description,
                        _ => _.OrganisationType.Code,
                        _ => _.OrganisationType.IsActive
                        );

                });
        }
    }
}