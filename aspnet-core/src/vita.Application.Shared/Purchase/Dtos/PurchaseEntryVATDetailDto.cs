using System;
using Abp.Application.Services.Dto;

namespace vita.Purchase.Dtos
{
    public class PurchaseEntryVATDetailDto : EntityDto<long>
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

    }
}