using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace vita.Sales.Dtos
{
    public class CreateOrEditSalesInvoiceDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? DateOfSupply { get; set; }

        [Required]
        public string InvoiceCurrencyCode { get; set; }

        public string CurrencyCodeOriginatingCountry { get; set; }

        public string PurchaseOrderId { get; set; }

        public string BillingReferenceId { get; set; }

        public string ContractId { get; set; }

        public DateTime? LatestDeliveryDate { get; set; }

        public string Location { get; set; }

        public string CustomerId { get; set; }

        public string Status { get; set; }

        public string Additional_Info { get; set; }

        public string PaymentType { get; set; }

        public string PdfUrl { get; set; }

        public string QrCodeUrl { get; set; }

        public string XMLUrl { get; set; }

        public string ArchivalUrl { get; set; }

        public string PreviousInvoiceHash { get; set; }

        public string PerviousXMLHash { get; set; }

        public string XMLHash { get; set; }

        public string PdfHash { get; set; }

        public string XMLbase64 { get; set; }

        public string PdfBase64 { get; set; }

        public bool IsArchived { get; set; }

        public int TransTypeCode { get; set; }

        public string TransTypeDescription { get; set; }

        public string AdvanceReferenceNumber { get; set; }

        public string Invoicetransactioncode { get; set; }

        public string BusinessProcessType { get; set; }

        public string InvoiceNotes { get; set; }

        public CreateOrEditSalesInvoicePartyDto Supplier { get; set; }
        public CreateOrEditSalesInvoicePartyDto Buyer { get; set; }
        public List<CreateOrEditSalesInvoiceItemDto> Items { get; set; }
        public CreateOrEditSalesInvoiceSummaryDto InvoiceSummary { get; set; }
        public List<CreateOrEditSalesInvoiceDiscountDto> Discount { get; set; }
        public List<CreateOrEditSalesInvoiceVATDetailDto> VATDetails { get; set; }
        public List<CreateOrEditSalesInvoicePaymentDetailDto> PaymentDetails { get; set; }

    }
}