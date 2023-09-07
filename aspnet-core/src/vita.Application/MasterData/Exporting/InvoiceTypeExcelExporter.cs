using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class InvoiceTypeExcelExporter : NpoiExcelExporterBase, IInvoiceTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public InvoiceTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetInvoiceTypeForViewDto> invoiceType)
        {
            return CreateExcelPackage(
                "InvoiceType.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("InvoiceType"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, invoiceType,
                        _ => _.InvoiceType.Name,
                        _ => _.InvoiceType.Description,
                        _ => _.InvoiceType.Code,
                        _ => _.InvoiceType.IsActive
                        );

                });
        }
    }
}