using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.PurchaseCredit.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.PurchaseCredit.Exporting
{
    public class PurchaseCreditNoteDiscountExcelExporter : NpoiExcelExporterBase, IPurchaseCreditNoteDiscountExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PurchaseCreditNoteDiscountExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPurchaseCreditNoteDiscountForViewDto> purchaseCreditNoteDiscount)
        {
            return CreateExcelPackage(
                    "PurchaseCreditNoteDiscount.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("PurchaseCreditNoteDiscount"));

                        AddHeader(
                            sheet,
                        L("IRNNo"),
                        L("DiscountPercentage"),
                        L("DiscountAmount"),
                        L("VATCode"),
                        L("VATRate"),
                        L("TaxSchemeId")
                            );

                        AddObjects(
                            sheet, purchaseCreditNoteDiscount,
                        _ => _.PurchaseCreditNoteDiscount.IRNNo,
                        _ => _.PurchaseCreditNoteDiscount.DiscountPercentage,
                        _ => _.PurchaseCreditNoteDiscount.DiscountAmount,
                        _ => _.PurchaseCreditNoteDiscount.VATCode,
                        _ => _.PurchaseCreditNoteDiscount.VATRate,
                        _ => _.PurchaseCreditNoteDiscount.TaxSchemeId
                            );

                    });

        }
    }
}