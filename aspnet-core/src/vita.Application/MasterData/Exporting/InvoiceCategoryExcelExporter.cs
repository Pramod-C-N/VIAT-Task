using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class InvoiceCategoryExcelExporter : NpoiExcelExporterBase, IInvoiceCategoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public InvoiceCategoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetInvoiceCategoryForViewDto> invoiceCategory)
        {
            return CreateExcelPackage(
                "InvoiceCategory.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("InvoiceCategory"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, invoiceCategory,
                        _ => _.InvoiceCategory.Name,
                        _ => _.InvoiceCategory.Description,
                        _ => _.InvoiceCategory.Code,
                        _ => _.InvoiceCategory.IsActive
                        );

                });
        }
    }
}