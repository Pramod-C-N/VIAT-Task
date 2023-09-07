using System;
using Abp.Application.Services.Dto;

namespace vita.DraftFee.Dtos
{
    public class DraftPaymentDetailDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

    }
}