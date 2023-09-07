using Abp.Application.Services.Dto;
using System;

namespace vita.DraftFee.Dtos
{
    public class GetAllDraftItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string IdentifierFilter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string BuyerIdentifierFilter { get; set; }

        public string SellerIdentifierFilter { get; set; }

        public string StandardIdentifierFilter { get; set; }

        public decimal? MaxQuantityFilter { get; set; }
        public decimal? MinQuantityFilter { get; set; }

        public string UOMFilter { get; set; }

        public decimal? MaxUnitPriceFilter { get; set; }
        public decimal? MinUnitPriceFilter { get; set; }

        public decimal? MaxCostPriceFilter { get; set; }
        public decimal? MinCostPriceFilter { get; set; }

        public decimal? MaxDiscountPercentageFilter { get; set; }
        public decimal? MinDiscountPercentageFilter { get; set; }

        public decimal? MaxDiscountAmountFilter { get; set; }
        public decimal? MinDiscountAmountFilter { get; set; }

        public decimal? MaxGrossPriceFilter { get; set; }
        public decimal? MinGrossPriceFilter { get; set; }

        public decimal? MaxNetPriceFilter { get; set; }
        public decimal? MinNetPriceFilter { get; set; }

        public decimal? MaxVATRateFilter { get; set; }
        public decimal? MinVATRateFilter { get; set; }

        public string VATCodeFilter { get; set; }

        public decimal? MaxVATAmountFilter { get; set; }
        public decimal? MinVATAmountFilter { get; set; }

        public decimal? MaxLineAmountInclusiveVATFilter { get; set; }
        public decimal? MinLineAmountInclusiveVATFilter { get; set; }

        public string CurrencyCodeFilter { get; set; }

        public string TaxSchemeIdFilter { get; set; }

        public string NotesFilter { get; set; }

        public string ExcemptionReasonCodeFilter { get; set; }

        public string ExcemptionReasonTextFilter { get; set; }

        public string AdditionalData2Filter { get; set; }

        public string LanguageFilter { get; set; }

    }
}