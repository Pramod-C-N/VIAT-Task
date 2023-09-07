using System;
using Abp.Application.Services.Dto;

namespace vita.Sales.Dtos
{
    public class SalesInvoiceDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? DateOfSupply { get; set; }

        public string InvoiceCurrencyCode { get; set; }

        public string CurrencyCodeOriginatingCountry { get; set; }

        public string PurchaseOrderId { get; set; }

        public string BillingReferenceId { get; set; }

        public string ContractId { get; set; }

        public DateTime? LatestDeliveryDate { get; set; }

        public string Location { get; set; }

        public string CustomerId { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public bool IsArchived { get; set; }

        public int TransTypeCode { get; set; }

        public string TransTypeDescription { get; set; }

        public string AdvanceReferenceNumber { get; set; }

        public string Invoicetransactioncode { get; set; }

        public string BusinessProcessType { get; set; }

        public string InvoiceNotes { get; set; }

        public string XmlUuid { get; set; }

        public string InvoiceTypeCode { get; set; }

        public string Language { get; set; }

        public string AdditionalData2 { get; set; }

        public string AdditionalData3 { get; set; }

        public string AdditionalData4 { get; set; }

    }
}