using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.PurchaseDebit
{
    [Table("PurchaseDebitNotePaymentDetail")]
    [Audited]
    public class PurchaseDebitNotePaymentDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string IRNNo { get; set; }

        public virtual string PaymentMeans { get; set; }

        public virtual string CreditDebitReasonText { get; set; }

        public virtual string PaymentTerms { get; set; }

    }
}