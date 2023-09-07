using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("ApportionmentBaseData")]
    [Audited]
    public class ApportionmentBaseData : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual DateTime? EffectiveFromDate { get; set; }

        public virtual DateTime? EffectiveTillEndDate { get; set; }

        public virtual decimal? TaxableSupply { get; set; }

        public virtual decimal? TotalSupply { get; set; }

        public virtual decimal? TaxablePurchase { get; set; }

        public virtual decimal? TotalPurchase { get; set; }

        public virtual string FinYear { get; set; }

        public virtual decimal? TotalExemptSales { get; set; }

        public virtual decimal? TotalExemptPurchase { get; set; }

        public virtual decimal? MixedOverHeads { get; set; }

        public virtual decimal? ApportionmentSupplies { get; set; }

        public virtual decimal? ApportionmentPurchases { get; set; }

    }
}