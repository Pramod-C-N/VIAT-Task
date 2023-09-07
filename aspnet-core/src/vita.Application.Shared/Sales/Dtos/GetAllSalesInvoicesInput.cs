using Abp.Application.Services.Dto;
using System;

namespace vita.Sales.Dtos
{
    public class GetAllSalesInvoicesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string InvoiceNumberFilter { get; set; }

        public DateTime? MaxIssueDateFilter { get; set; }
        public DateTime? MinIssueDateFilter { get; set; }

        public DateTime? MaxDateOfSupplyFilter { get; set; }
        public DateTime? MinDateOfSupplyFilter { get; set; }

        public string InvoiceCurrencyCodeFilter { get; set; }

        public string CurrencyCodeOriginatingCountryFilter { get; set; }

        public string PurchaseOrderIdFilter { get; set; }

        public string BillingReferenceIdFilter { get; set; }

        public string ContractIdFilter { get; set; }

        public DateTime? MaxLatestDeliveryDateFilter { get; set; }
        public DateTime? MinLatestDeliveryDateFilter { get; set; }

        public string LocationFilter { get; set; }

        public string CustomerIdFilter { get; set; }

        public string StatusFilter { get; set; }

        public string PaymentTypeFilter { get; set; }

        public int? IsArchivedFilter { get; set; }

        public int? MaxTransTypeCodeFilter { get; set; }
        public int? MinTransTypeCodeFilter { get; set; }

        public string TransTypeDescriptionFilter { get; set; }

        public string AdvanceReferenceNumberFilter { get; set; }

        public string InvoicetransactioncodeFilter { get; set; }

        public string BusinessProcessTypeFilter { get; set; }

        public string InvoiceNotesFilter { get; set; }

    }
}