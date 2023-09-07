using Abp.Application.Services.Dto;
using System;

namespace vita.Debit.Dtos
{
    public class GetAllDebitNoteDiscountsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public decimal? MaxDiscountPercentageFilter { get; set; }
        public decimal? MinDiscountPercentageFilter { get; set; }

        public decimal? MaxDiscountAmountFilter { get; set; }
        public decimal? MinDiscountAmountFilter { get; set; }

        public string VATCodeFilter { get; set; }

        public decimal? MaxVATRateFilter { get; set; }
        public decimal? MinVATRateFilter { get; set; }

        public string TaxSchemeIdFilter { get; set; }

    }
}