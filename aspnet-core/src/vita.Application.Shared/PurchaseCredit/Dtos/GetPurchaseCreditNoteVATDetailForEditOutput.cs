using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseCredit.Dtos
{
    public class GetPurchaseCreditNoteVATDetailForEditOutput
    {
        public CreateOrEditPurchaseCreditNoteVATDetailDto PurchaseCreditNoteVATDetail { get; set; }

    }
}