﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace vita.DraftFee.Dtos
{
    public class CreateOrEditDraftDto : EntityDto<long?>
    {

        public Guid UniqueIdentifier { get; set; }

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

        public string XmlUuid { get; set; }

        public string AdditionalData1 { get; set; }

        public string AdditionalData2 { get; set; }

        public string AdditionalData3 { get; set; }

        public string AdditionalData4 { get; set; }

        public string InvoiceTypeCode { get; set; }

        public string Language { get; set; }

        public bool isSent { get; set; }

        public string Error { get; set; }

        public string DraftStatus { get; set; }

        public string Source { get; set; }

        public List<CreateOrEditDraftPartyDto> Supplier { get; set; } = new List<CreateOrEditDraftPartyDto>();
        public List<CreateOrEditDraftPartyDto> Buyer { get; set; } = new List<CreateOrEditDraftPartyDto>();
        public List<CreateOrEditDraftPartyDto> Delivery { get; set; } = new List<CreateOrEditDraftPartyDto>();
        public List<CreateOrEditDraftItemDto> Items { get; set; }
        public CreateOrEditDraftSummaryDto InvoiceSummary { get; set; }
        public List<CreateOrEditDraftDiscountDto> Discount { get; set; }
        public List<CreateOrEditDraftVATDetailDto> VATDetails { get; set; }
        public List<CreateOrEditDraftPaymentDetailDto> PaymentDetails { get; set; }

    }
}