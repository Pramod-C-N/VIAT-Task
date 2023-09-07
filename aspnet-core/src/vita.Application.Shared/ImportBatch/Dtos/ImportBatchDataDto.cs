using System;
using Abp.Application.Services.Dto;

namespace vita.ImportBatch.Dtos
{
    public class ImportBatchDataDto : EntityDto<long>
    {
        public int BatchId { get; set; }

        public string Filename { get; set; }

        public string InvoiceType { get; set; }

        public string IRNNo { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime? IssueDate { get; set; }

        public string IssueTime { get; set; }

        public string InvoiceCurrencyCode { get; set; }

        public string PurchaseOrderId { get; set; }

        public string ContractId { get; set; }

        public DateTime? SupplyDate { get; set; }

        public DateTime? SupplyEndDate { get; set; }

        public string BuyerMasterCode { get; set; }

        public string BuyerName { get; set; }

        public string BuyerVatCode { get; set; }

        public string BuyerContact { get; set; }

        public string BuyerCountryCode { get; set; }

        public string InvoiceLineIdentifier { get; set; }

        public string ItemMasterCode { get; set; }

        public string ItemName { get; set; }

        public string UOM { get; set; }

        public decimal? GrossPrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? NetPrice { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? LineNetAmount { get; set; }

        public string VatCategoryCode { get; set; }

        public decimal? VatRate { get; set; }

        public string VatExemptionReasonCode { get; set; }

        public string VatExemptionReason { get; set; }

        public decimal? VATLineAmount { get; set; }

        public decimal? LineAmountInclusiveVAT { get; set; }

        public int Processed { get; set; }

        public string Error { get; set; }

        public string BillingReferenceId { get; set; }

        public DateTime? OrignalSupplyDate { get; set; }

        public string ReasonForCN { get; set; }

        public string BillOfEntry { get; set; }

        public DateTime? BillOfEntryDate { get; set; }

        public decimal? CustomsPaid { get; set; }

        public decimal? CustomTax { get; set; }

        public bool WHTApplicable { get; set; }

        public string PurchaseNumber { get; set; }

        public string PurchaseCategory { get; set; }

        public string LedgerHeader { get; set; }

        public string TransType { get; set; }

        public decimal? AdvanceRcptAmtAdjusted { get; set; }

        public decimal? VatOnAdvanceRcptAmtAdjusted { get; set; }

        public string AdvanceRcptRefNo { get; set; }

        public string PaymentMeans { get; set; }

        public string PaymentTerms { get; set; }

        public string NatureofServices { get; set; }

        public bool Isapportionment { get; set; }

        public decimal? ExciseTaxPaid { get; set; }

        public decimal? OtherChargesPaid { get; set; }

        public decimal? TotalTaxableAmount { get; set; }

        public bool VATDeffered { get; set; }

        public string PlaceofSupply { get; set; }

        public bool RCMApplicable { get; set; }

        public string OrgType { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string AffiliationStatus { get; set; }

        public decimal? ReferenceInvoiceAmount { get; set; }

        public string PerCapitaHoldingForiegnCo { get; set; }

        public string CapitalInvestedbyForeignCompany { get; set; }

        public string CapitalInvestmentCurrency { get; set; }

        public DateTime? CapitalInvestmentDate { get; set; }

        public string VendorConstitution { get; set; }

    }
}