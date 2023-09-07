using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class AllowanceReasonExcelExporter : NpoiExcelExporterBase, IAllowanceReasonExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AllowanceReasonExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAllowanceReasonForViewDto> allowanceReason)
        {
            return CreateExcelPackage(
                "AllowanceReason.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("AllowanceReason"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, allowanceReason,
                        _ => _.AllowanceReason.Name,
                        _ => _.AllowanceReason.Description,
                        _ => _.AllowanceReason.Code,
                        _ => _.AllowanceReason.IsActive
                        );

                });
        }
    }
}