using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class PurchaseTypeExcelExporter : NpoiExcelExporterBase, IPurchaseTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PurchaseTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPurchaseTypeForViewDto> purchaseType)
        {
            return CreateExcelPackage(
                "PurchaseType.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PurchaseType"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, purchaseType,
                        _ => _.PurchaseType.Name,
                        _ => _.PurchaseType.Description,
                        _ => _.PurchaseType.Code,
                        _ => _.PurchaseType.IsActive
                        );

                });
        }
    }
}