using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Purchase
{
    [Table("PurchaseEntryPaymentDetail")]
    [Audited]
    public class PurchaseEntryPaymentDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual string PaymentMeans { get; set; }

        public virtual string CreditDebitReasonText { get; set; }

        public virtual string PaymentTerms { get; set; }

    }
}