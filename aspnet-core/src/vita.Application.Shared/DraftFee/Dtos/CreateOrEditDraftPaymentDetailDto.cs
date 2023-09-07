using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.DraftFee.Dtos
{
    public class CreateOrEditDraftPaymentDetailDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

    }
}