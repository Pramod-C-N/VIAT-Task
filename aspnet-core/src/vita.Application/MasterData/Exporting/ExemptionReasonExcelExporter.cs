using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ExemptionReasonExcelExporter : NpoiExcelExporterBase, IExemptionReasonExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExemptionReasonExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExemptionReasonForViewDto> exemptionReason)
        {
            return CreateExcelPackage(
                "ExemptionReason.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ExemptionReason"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, exemptionReason,
                        _ => _.ExemptionReason.Name,
                        _ => _.ExemptionReason.Description,
                        _ => _.ExemptionReason.Code,
                        _ => _.ExemptionReason.IsActive
                        );

                });
        }
    }
}