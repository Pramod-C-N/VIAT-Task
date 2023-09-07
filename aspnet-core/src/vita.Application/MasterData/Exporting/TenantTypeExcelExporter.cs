using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class TenantTypeExcelExporter : NpoiExcelExporterBase, ITenantTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantTypeForViewDto> tenantType)
        {
            return CreateExcelPackage(
                "TenantType.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantType"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, tenantType,
                        _ => _.TenantType.Name,
                        _ => _.TenantType.Description,
                        _ => _.TenantType.IsActive
                        );

                });
        }
    }
}