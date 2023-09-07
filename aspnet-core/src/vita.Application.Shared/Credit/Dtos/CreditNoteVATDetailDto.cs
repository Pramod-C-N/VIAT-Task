using System;
using Abp.Application.Services.Dto;

namespace vita.Credit.Dtos
{
    public class CreditNoteVATDetailDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string TaxSchemeId { get; set; }

        public string VATCode { get; set; }

        public decimal VATRate { get; set; }

        public string ExcemptionReasonCode { get; set; }

        public string ExcemptionReasonText { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public string CurrencyCode { get; set; }

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

    }
}