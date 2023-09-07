using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.ImportBatch
{
    [Table("ImportBatchData")]
    [Audited]
    public class ImportBatchData : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual int BatchId { get; set; }

        public virtual string Filename { get; set; }

        public virtual string InvoiceType { get; set; }

        public virtual string IRNNo { get; set; }

        public virtual string InvoiceNumber { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual string IssueTime { get; set; }

        public virtual string InvoiceCurrencyCode { get; set; }

        public virtual string PurchaseOrderId { get; set; }

        public virtual string ContractId { get; set; }

        public virtual DateTime? SupplyDate { get; set; }

        public virtual DateTime? SupplyEndDate { get; set; }

        public virtual string BuyerMasterCode { get; set; }

        public virtual string BuyerName { get; set; }

        public virtual string BuyerVatCode { get; set; }

        public virtual string BuyerContact { get; set; }

        public virtual string BuyerCountryCode { get; set; }

        public virtual string InvoiceLineIdentifier { get; set; }

        public virtual string ItemMasterCode { get; set; }

        public virtual string ItemName { get; set; }

        public virtual string UOM { get; set; }

        public virtual decimal? GrossPrice { get; set; }

        public virtual decimal? Discount { get; set; }

        public virtual decimal? NetPrice { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? LineNetAmount { get; set; }

        public virtual string VatCategoryCode { get; set; }

        public virtual decimal? VatRate { get; set; }

        public virtual string VatExemptionReasonCode { get; set; }

        public virtual string VatExemptionReason { get; set; }

        public virtual decimal? VATLineAmount { get; set; }

        public virtual decimal? LineAmountInclusiveVAT { get; set; }

        public virtual int Processed { get; set; }

        public virtual string Error { get; set; }

        public virtual string BillingReferenceId { get; set; }

        public virtual DateTime? OrignalSupplyDate { get; set; }

        public virtual string ReasonForCN { get; set; }

        public virtual string BillOfEntry { get; set; }

        public virtual DateTime? BillOfEntryDate { get; set; }

        public virtual decimal? CustomsPaid { get; set; }

        public virtual decimal? CustomTax { get; set; }

        public virtual bool WHTApplicable { get; set; }

        public virtual string PurchaseNumber { get; set; }

        public virtual string PurchaseCategory { get; set; }

        public virtual string LedgerHeader { get; set; }

        public virtual string TransType { get; set; }

        public virtual decimal? AdvanceRcptAmtAdjusted { get; set; }

        public virtual decimal? VatOnAdvanceRcptAmtAdjusted { get; set; }

        public virtual string AdvanceRcptRefNo { get; set; }

        public virtual string PaymentMeans { get; set; }

        public virtual string PaymentTerms { get; set; }

        public virtual string NatureofServices { get; set; }

        public virtual bool Isapportionment { get; set; }

        public virtual decimal? ExciseTaxPaid { get; set; }

        public virtual decimal? OtherChargesPaid { get; set; }

        public virtual decimal? TotalTaxableAmount { get; set; }

        public virtual bool VATDeffered { get; set; }

        public virtual string PlaceofSupply { get; set; }

        public virtual bool RCMApplicable { get; set; }

        public virtual string OrgType { get; set; }

        public virtual decimal? ExchangeRate { get; set; }

        public virtual string AffiliationStatus { get; set; }

        public virtual decimal? ReferenceInvoiceAmount { get; set; }

        public virtual string PerCapitaHoldingForiegnCo { get; set; }

        public virtual string CapitalInvestedbyForeignCompany { get; set; }

        public virtual string CapitalInvestmentCurrency { get; set; }

        public virtual DateTime? CapitalInvestmentDate { get; set; }

        public virtual string VendorConstitution { get; set; }

    }
}