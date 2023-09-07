using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ErrorGroupExcelExporter : NpoiExcelExporterBase, IErrorGroupExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ErrorGroupExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetErrorGroupForViewDto> errorGroup)
        {
            return CreateExcelPackage(
                "ErrorGroup.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ErrorGroup"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, errorGroup,
                        _ => _.ErrorGroup.Name,
                        _ => _.ErrorGroup.Description,
                        _ => _.ErrorGroup.Code,
                        _ => _.ErrorGroup.IsActive
                        );

                });
        }
    }
}