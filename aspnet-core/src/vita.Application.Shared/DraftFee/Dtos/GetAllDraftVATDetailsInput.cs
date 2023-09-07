using Abp.Application.Services.Dto;
using System;

namespace vita.DraftFee.Dtos
{
    public class GetAllDraftVATDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string TaxSchemeIdFilter { get; set; }

        public string VATCodeFilter { get; set; }

        public decimal? MaxVATRateFilter { get; set; }
        public decimal? MinVATRateFilter { get; set; }

        public string ExcemptionReasonCodeFilter { get; set; }

        public string ExcemptionReasonTextFilter { get; set; }

        public decimal? MaxTaxableAmountFilter { get; set; }
        public decimal? MinTaxableAmountFilter { get; set; }

        public decimal? MaxTaxAmountFilter { get; set; }
        public decimal? MinTaxAmountFilter { get; set; }

        public string CurrencyCodeFilter { get; set; }

        public string AdditionalData1Filter { get; set; }

        public string LanguageFilter { get; set; }

    }
}