using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using vita.Sales.Dtos;
using System.Collections.Generic;

namespace vita.Debit.Dtos
{
    public class CreateOrEditDebitNoteDto : EntityDto<long?>
    {
        
        public string IRNNo { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DateOfSupply { get; set; }

        public string InvoiceCurrencyCode { get; set; }

        public string CurrencyCodeOriginatingCountry { get; set; }

        public string PurchaseOrderId { get; set; }

        public string BillingReferenceId { get; set; }

        public string ContractId { get; set; }

        public DateTime LatestDeliveryDate { get; set; }

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
        public CreateOrEditDebitNotePartyDto Supplier { get; set; }
        public CreateOrEditDebitNotePartyDto Buyer { get; set; }
        public List<CreateOrEditDebitNoteItemDto> Items { get; set; }
        public CreateOrEditDebitNoteSummaryDto InvoiceSummary { get; set; }
        public List<CreateOrEditDebitNoteDiscountDto> Discount { get; set; }
        public List<CreateOrEditDebitNoteVATDetailDto> VATDetails { get; set; }
        public List<CreateOrEditDebitNotePaymentDetailDto> PaymentDetails { get; set; }

    }
}