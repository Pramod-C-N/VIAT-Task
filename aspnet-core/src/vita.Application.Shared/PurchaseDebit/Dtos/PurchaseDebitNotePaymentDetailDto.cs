using System;
using Abp.Application.Services.Dto;

namespace vita.PurchaseDebit.Dtos
{
    public class PurchaseDebitNotePaymentDetailDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}