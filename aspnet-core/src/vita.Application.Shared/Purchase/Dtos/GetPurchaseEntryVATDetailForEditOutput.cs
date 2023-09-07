using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Purchase.Dtos
{
    public class GetPurchaseEntryVATDetailForEditOutput
    {
        public CreateOrEditPurchaseEntryVATDetailDto PurchaseEntryVATDetail { get; set; }

    }
}