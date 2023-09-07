using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseCredit.Dtos
{
    public class GetPurchaseCreditNoteDiscountForEditOutput
    {
        public CreateOrEditPurchaseCreditNoteDiscountDto PurchaseCreditNoteDiscount { get; set; }

    }
}