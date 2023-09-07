using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class NatureofServicesExcelExporter : NpoiExcelExporterBase, INatureofServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NatureofServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNatureofServicesForViewDto> natureofServices)
        {
            return CreateExcelPackage(
                "NatureofServices.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("NatureofServices"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, natureofServices,
                        _ => _.NatureofServices.Name,
                        _ => _.NatureofServices.Description,
                        _ => _.NatureofServices.Code,
                        _ => _.NatureofServices.IsActive
                        );

                });
        }
    }
}