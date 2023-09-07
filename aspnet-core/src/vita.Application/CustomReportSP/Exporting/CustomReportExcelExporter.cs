using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.CustomReportSP.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.CustomReportSP.Exporting
{
    public class CustomReportExcelExporter : NpoiExcelExporterBase, ICustomReportExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomReportExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCustomReportForViewDto> customReport)
        {
            return CreateExcelPackage(
                "CustomReport.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CustomReport"));

                    AddHeader(
                        sheet,
                        L("ReportName"),
                        L("StoredProcedureName")
                        );

                    AddObjects(
                        sheet, customReport,
                        _ => _.CustomReport.ReportName,
                        _ => _.CustomReport.StoredProcedureName
                        );

                });
        }
    }
}