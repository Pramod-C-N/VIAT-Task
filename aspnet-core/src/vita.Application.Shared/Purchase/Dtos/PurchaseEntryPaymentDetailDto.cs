using System;
using Abp.Application.Services.Dto;

namespace vita.Purchase.Dtos
{
    public class PurchaseEntryPaymentDetailDto : EntityDto<long>
    {
        public Guid UniqueIdentifier { get; set; }

        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}