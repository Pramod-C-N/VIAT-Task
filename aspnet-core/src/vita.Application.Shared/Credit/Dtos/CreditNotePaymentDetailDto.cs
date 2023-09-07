using System;
using Abp.Application.Services.Dto;

namespace vita.Credit.Dtos
{
    public class CreditNotePaymentDetailDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}