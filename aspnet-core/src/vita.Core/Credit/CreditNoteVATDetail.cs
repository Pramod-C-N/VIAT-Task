using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Credit
{
    [Table("CreditNoteVATDetail")]
    [Audited]
    public class CreditNoteVATDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual string TaxSchemeId { get; set; }

        public virtual string VATCode { get; set; }

        public virtual decimal VATRate { get; set; }

        public virtual string ExcemptionReasonCode { get; set; }

        public virtual string ExcemptionReasonText { get; set; }

        public virtual decimal TaxableAmount { get; set; }

        public virtual decimal TaxAmount { get; set; }

        public virtual string CurrencyCode { get; set; }

    }
}