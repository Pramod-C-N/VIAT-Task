using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Debit
{
    [Table("DebitNoteItem")]
    [Audited]
    public class DebitNoteItem : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual string Identifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string BuyerIdentifier { get; set; }

        public virtual string SellerIdentifier { get; set; }

        public virtual string StandardIdentifier { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string UOM { get; set; }

        public virtual decimal UnitPrice { get; set; }

        public virtual decimal CostPrice { get; set; }

        public virtual decimal DiscountPercentage { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal GrossPrice { get; set; }

        public virtual decimal NetPrice { get; set; }

        public virtual decimal VATRate { get; set; }

        public virtual string VATCode { get; set; }

        public virtual decimal VATAmount { get; set; }

        public virtual decimal LineAmountInclusiveVAT { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual string TaxSchemeId { get; set; }

        public virtual string Notes { get; set; }

        public virtual string ExcemptionReasonCode { get; set; }

        public virtual string ExcemptionReasonText { get; set; }

    }
}