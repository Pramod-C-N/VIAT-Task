using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace vita.PurchaseFileUpload
{
    [Table("ImportPurchaseFiles")]
    public class ImportPurchaseFile : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual int BatchId { get; set; }

        public virtual string FileName { get; set; }

        public virtual string InvoiceType { get; set; }

        public virtual string IRNNo { get; set; }

        public virtual string InvoiceNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual string IssueTime { get; set; }

        public virtual string InvoiceCurrencyCode { get; set; }

        public virtual string PurchaseOrderId { get; set; }

        public virtual string ContractId { get; set; }

        public virtual DateTime SupplyDate { get; set; }

        public virtual DateTime SupplyEndDate { get; set; }

        public virtual string BuyerMasterCode { get; set; }

        public virtual string BuyerName { get; set; }

        public virtual string BuyerVatCode { get; set; }

        public virtual string BuyerContact { get; set; }

        public virtual string BuyerCountryCode { get; set; }

        public virtual string InvoiceLineIdentifier { get; set; }

        public virtual string ItemMasterCode { get; set; }

        public virtual string ItemName { get; set; }

        public virtual string UOM { get; set; }

        public virtual decimal GrossPrice { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual decimal NetPrice { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual decimal LineNetAmount { get; set; }

        public virtual string VatCategoryCode { get; set; }

        public virtual decimal VatRate { get; set; }

        public virtual string VatExemptionReasonCode { get; set; }

        public virtual string VatExemptionReason { get; set; }

        public virtual decimal VATLineAmount { get; set; }

        public virtual decimal LineAmountInclusiveVAT { get; set; }

        public virtual bool Processed { get; set; }

        public virtual string Error { get; set; }

        public virtual string LedgerHead { get; set; }

    }
}