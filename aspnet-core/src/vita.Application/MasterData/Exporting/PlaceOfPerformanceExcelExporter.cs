using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class PlaceOfPerformanceExcelExporter : NpoiExcelExporterBase, IPlaceOfPerformanceExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PlaceOfPerformanceExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPlaceOfPerformanceForViewDto> placeOfPerformance)
        {
            return CreateExcelPackage(
                "PlaceOfPerformance.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PlaceOfPerformance"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, placeOfPerformance,
                        _ => _.PlaceOfPerformance.Name,
                        _ => _.PlaceOfPerformance.Description,
                        _ => _.PlaceOfPerformance.Code,
                        _ => _.PlaceOfPerformance.IsActive
                        );

                });
        }
    }
}