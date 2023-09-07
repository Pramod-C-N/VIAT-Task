using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Purchase.Dtos
{
    public class GetPurchaseEntryPaymentDetailForEditOutput
    {
        public CreateOrEditPurchaseEntryPaymentDetailDto PurchaseEntryPaymentDetail { get; set; }

    }
}