using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ErrorTypeExcelExporter : NpoiExcelExporterBase, IErrorTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ErrorTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetErrorTypeForViewDto> errorType)
        {
            return CreateExcelPackage(
                "ErrorType.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ErrorType"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("ModuleName"),
                        L("ErrorGroupId"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, errorType,
                        _ => _.ErrorType.Name,
                        _ => _.ErrorType.Description,
                        _ => _.ErrorType.Code,
                        _ => _.ErrorType.ModuleName,
                        _ => _.ErrorType.ErrorGroupId,
                        _ => _.ErrorType.IsActive
                        );

                });
        }
    }
}