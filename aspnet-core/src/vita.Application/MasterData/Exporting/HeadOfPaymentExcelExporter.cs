using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class HeadOfPaymentExcelExporter : NpoiExcelExporterBase, IHeadOfPaymentExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HeadOfPaymentExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHeadOfPaymentForViewDto> headOfPayment)
        {
            return CreateExcelPackage(
                "HeadOfPayment.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HeadOfPayment"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("NatureOfService"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, headOfPayment,
                        _ => _.HeadOfPayment.Name,
                        _ => _.HeadOfPayment.Description,
                        _ => _.HeadOfPayment.Code,
                        _ => _.HeadOfPayment.NatureOfService,
                        _ => _.HeadOfPayment.IsActive
                        );

                });
        }
    }
}