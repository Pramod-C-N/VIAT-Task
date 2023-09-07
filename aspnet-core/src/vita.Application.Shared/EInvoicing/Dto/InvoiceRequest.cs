using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using vita.Sales.Dtos;

namespace vita.EInvoicing.Dto
{
    public class InvoiceXMLRequest
    {
        public string xmlString { get; set; }

        public InvoiceRequest InvoiceRequest { get; set; }

        public bool isParsed { get; set; } = false;

    }

    public class PDFRequest
    {
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
        public string InvoiceNotes { get; set; }

        public string PaymentType { get; set; }
        public List<InvoicePartyDto> Supplier { get; set; }
        public List<InvoicePartyDto> Buyer { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
        public InvoiceSummaryDto InvoiceSummary { get; set; }
        public List<InvoiceDiscountDto> Discount { get; set; }
        public List<InvoiceVATDetailDto> VATDetails { get; set; }
        public List<InvoicePaymentDetailDto> PaymentDetails { get; set; }
        public InvoiceTypeEnum? InvoiceType { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }
        public List<object> AdditionalData2 { get; set; }
        public List<object> AdditionalData3 { get; set; }
        public List<object> AdditionalData4 { get; set; }
    }

    public class InvoiceRequest
    {
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
        public string InvoiceNotes { get; set; }

        public string PaymentType { get; set; }
        public List<InvoicePartyDto> Supplier { get; set; }
        public List<InvoicePartyDto> Buyer { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
        public InvoiceSummaryDto InvoiceSummary { get; set; }
        public List<InvoiceDiscountDto> Discount { get; set; }
        public List<InvoiceVATDetailDto> VATDetails { get; set; }
        public List<InvoicePaymentDetailDto> PaymentDetails { get; set; }
        public InvoiceTypeEnum? InvoiceType { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }
        public List<object> AdditionalData2 { get; set; }
        public List<object> AdditionalData3 { get; set; }
        public List<object> AdditionalData4 { get; set; }
    }

    public class InvoiceRequestLanguage
    {
        public bool GenerateDraft { get; set; } = true;
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
        public string InvoiceNotes { get; set; }

        public string PaymentType { get; set; }
        public List<InvoicePartyDto> Supplier { get; set; }
        public List<InvoicePartyDto> Buyer { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
        public InvoiceSummaryDto InvoiceSummary { get; set; }
        public List<InvoiceDiscountDto> Discount { get; set; }
        public List<InvoiceVATDetailDto> VATDetails { get; set; }
        public List<InvoicePaymentDetailDto> PaymentDetails { get; set; }
        public InvoiceTypeEnum? InvoiceType { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }
        public List<object> AdditionalData2 { get; set; }
        public List<object> AdditionalData3 { get; set; }
        public List<object> AdditionalData4 { get; set; }
    }

    public class InvoicePaymentDetailDto
    {

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }
    }

    public class InvoiceVATDetailDto
    {
        public string TaxSchemeId { get; set; }

        public string VATCode { get; set; }

        public decimal VATRate { get; set; }

        public string ExcemptionReasonCode { get; set; }

        public string ExcemptionReasonText { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public string CurrencyCode { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }

    }

    public class InvoiceDiscountDto
    {

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public string VATCode { get; set; }

        public decimal VATRate { get; set; }

        public string TaxSchemeId { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }

    }
    public class InvoiceSummaryDto
    {

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
        public List<object> AdditionalData1 { get; set; }
    }
    public class InvoiceItemDto
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BuyerIdentifier { get; set; }

        public string SellerIdentifier { get; set; }

        public string StandardIdentifier { get; set; }

        public decimal Quantity { get; set; }

        public string UOM { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal GrossPrice { get; set; }

        public decimal NetPrice { get; set; }

        public decimal VATRate { get; set; }

        public string VATCode { get; set; }

        public decimal VATAmount { get; set; }

        public decimal LineAmountInclusiveVAT { get; set; }

        public string CurrencyCode { get; set; }

        public string TaxSchemeId { get; set; }

        public string Notes { get; set; }

        public string ExcemptionReasonCode { get; set; }

        public string ExcemptionReasonText { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }
        public List<object> AdditionalData2 { get; set; }

        public bool isOtherCharges { get; set; }

    }
    public class InvoicePartyDto
    {

        public string RegistrationName { get; set; }

        public string VATID { get; set; }

        public string GroupVATID { get; set; }

        public string CRNumber { get; set; }

        public string OtherID { get; set; }

        public string CustomerId { get; set; }

        public string Type { get; set; }
        public string FaxNo { get; set; }

        public string Website { get; set; }
        public InvoiceAddressDto Address { get; set; }
        public InvoiceContactPersonDto ContactPerson { get; set; }

        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }

    }
    public class InvoiceAddressDto
    {
        public string Street { get; set; }

        public string AdditionalStreet { get; set; }

        public string BuildingNo { get; set; }

        public string AdditionalNo { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string State { get; set; }

        public string Neighbourhood { get; set; }

        public string CountryCode { get; set; }

        public string Type { get; set; }

        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }

    }
    public class InvoiceContactPersonDto
    {

        public string Name { get; set; }

        public string EmployeeCode { get; set; }

        public string ContactNumber { get; set; }

        public string GovtId { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }
        public string Language { get; set; }
        public List<object> AdditionalData1 { get; set; }

    }
}
