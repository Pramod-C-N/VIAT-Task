using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ReasonCNDNExcelExporter : NpoiExcelExporterBase, IReasonCNDNExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReasonCNDNExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReasonCNDNForViewDto> reasonCNDN)
        {
            return CreateExcelPackage(
                "ReasonCNDN.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ReasonCNDN"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, reasonCNDN,
                        _ => _.ReasonCNDN.Name,
                        _ => _.ReasonCNDN.Description,
                        _ => _.ReasonCNDN.Code,
                        _ => _.ReasonCNDN.IsActive
                        );

                });
        }
    }
}