using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using vita.Credit.Dtos;
using System.Collections.Generic;

namespace vita.Purchase.Dtos
{
    public class CreateOrEditPurchaseEntryDto : EntityDto<long?>
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

        public string PurchaseNumber { get; set; }

        public DateTime SupplierInvoiceDate { get; set; }

        public string BillOfEntry { get; set; }

        public DateTime BillOfEntryDate { get; set; }

        public decimal CustomsPaid { get; set; }

        public decimal CustomTax { get; set; }

        public bool IsWHT { get; set; }

        public bool VATDeffered { get; set; }

        public string PlaceofSupply { get; set; }
        public CreateOrEditPurchaseEntryPartyDto Supplier { get; set; }
        public CreateOrEditPurchaseEntryPartyDto Buyer { get; set; }
        public List<CreateOrEditPurchaseEntryItemDto> Items { get; set; }
        public CreateOrEditPurchaseEntrySummaryDto InvoiceSummary { get; set; }
        public List<CreateOrEditPurchaseEntryDiscountDto> Discount { get; set; }
        public List<CreateOrEditPurchaseEntryVATDetailDto> VATDetails { get; set; }
        public List<CreateOrEditPurchaseEntryPaymentDetailDto> PaymentDetails { get; set; }

        public string Additional_Data { get; set; }

    }
}