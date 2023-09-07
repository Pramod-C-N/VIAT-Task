using System;
using Abp.Application.Services.Dto;

namespace vita.Sales.Dtos
{
    public class SalesInvoiceSummaryDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public decimal NetInvoiceAmount { get; set; }

        public string NetInvoiceAmountCurrency { get; set; }

        public decimal SumOfInvoiceLineNetAmount { get; set; }

        public string SumOfInvoiceLineNetAmountCurrency { get; set; }

        public decimal TotalAmountWithoutVAT { get; set; }

        public string TotalAmountWithoutVATCurrency { get; set; }

        public decimal TotalVATAmount { get; set; }

        public string CurrencyCode { get; set; }

        public decimal TotalAmountWithVAT { get; set; }

        public decimal PaidAmount { get; set; }

        public string PaidAmountCurrency { get; set; }

        public decimal PayableAmount { get; set; }

        public string PayableAmountCurrency { get; set; }

        public decimal AdvanceAmountwithoutVat { get; set; }

        public decimal AdvanceVat { get; set; }

    }
}