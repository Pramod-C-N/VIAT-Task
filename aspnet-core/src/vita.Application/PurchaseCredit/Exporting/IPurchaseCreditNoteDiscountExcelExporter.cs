using System.Collections.Generic;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit.Exporting
{
    public interface IPurchaseCreditNoteDiscountExcelExporter
    {
        FileDto ExportToFile(List<GetPurchaseCreditNoteDiscountForViewDto> purchaseCreditNoteDiscount);
    }
}