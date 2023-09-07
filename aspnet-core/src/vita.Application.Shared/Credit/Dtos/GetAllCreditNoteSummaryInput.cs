using Abp.Application.Services.Dto;
using System;

namespace vita.Credit.Dtos
{
    public class GetAllCreditNoteSummaryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public decimal? MaxNetInvoiceAmountFilter { get; set; }
        public decimal? MinNetInvoiceAmountFilter { get; set; }

        public string NetInvoiceAmountCurrencyFilter { get; set; }

        public decimal? MaxSumOfInvoiceLineNetAmountFilter { get; set; }
        public decimal? MinSumOfInvoiceLineNetAmountFilter { get; set; }

        public string SumOfInvoiceLineNetAmountCurrencyFilter { get; set; }

        public decimal? MaxTotalAmountWithoutVATFilter { get; set; }
        public decimal? MinTotalAmountWithoutVATFilter { get; set; }

        public string TotalAmountWithoutVATCurrencyFilter { get; set; }

        public decimal? MaxTotalVATAmountFilter { get; set; }
        public decimal? MinTotalVATAmountFilter { get; set; }

        public string CurrencyCodeFilter { get; set; }

        public decimal? MaxTotalAmountWithVATFilter { get; set; }
        public decimal? MinTotalAmountWithVATFilter { get; set; }

        public decimal? MaxPaidAmountFilter { get; set; }
        public decimal? MinPaidAmountFilter { get; set; }

        public string PaidAmountCurrencyFilter { get; set; }

        public decimal? MaxPayableAmountFilter { get; set; }
        public decimal? MinPayableAmountFilter { get; set; }

        public string PayableAmountCurrencyFilter { get; set; }

        public decimal? MaxAdvanceAmountwithoutVatFilter { get; set; }
        public decimal? MinAdvanceAmountwithoutVatFilter { get; set; }

        public decimal? MaxAdvanceVatFilter { get; set; }
        public decimal? MinAdvanceVatFilter { get; set; }

    }
}