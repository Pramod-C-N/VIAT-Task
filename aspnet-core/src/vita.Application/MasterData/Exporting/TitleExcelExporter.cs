using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class TitleExcelExporter : NpoiExcelExporterBase, ITitleExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TitleExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTitleForViewDto> title)
        {
            return CreateExcelPackage(
                "Title.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Title"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, title,
                        _ => _.Title.Name,
                        _ => _.Title.Description,
                        _ => _.Title.IsActive
                        );

                });
        }
    }
}