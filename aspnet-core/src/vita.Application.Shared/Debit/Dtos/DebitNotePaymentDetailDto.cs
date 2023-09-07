using System;
using Abp.Application.Services.Dto;

namespace vita.Debit.Dtos
{
    public class DebitNotePaymentDetailDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

    }
}