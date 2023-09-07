using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class PaymentMeansExcelExporter : NpoiExcelExporterBase, IPaymentMeansExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PaymentMeansExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPaymentMeansForViewDto> paymentMeans)
        {
            return CreateExcelPackage(
                "PaymentMeans.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PaymentMeans"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, paymentMeans,
                        _ => _.PaymentMeans.Name,
                        _ => _.PaymentMeans.Description,
                        _ => _.PaymentMeans.Code,
                        _ => _.PaymentMeans.IsActive
                        );

                });
        }
    }
}