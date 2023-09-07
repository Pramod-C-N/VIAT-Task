using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.PurchaseCredit
{
    [Table("PurchaseCreditNotePartiy")]
    [Audited]
    public class PurchaseCreditNoteParty : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string IRNNo { get; set; }

        public virtual string RegistrationName { get; set; }

        public virtual string VATID { get; set; }

        public virtual string GroupVATID { get; set; }

        public virtual string CRNumber { get; set; }

        public virtual string OtherID { get; set; }

        public virtual string CustomerId { get; set; }

        public virtual string Type { get; set; }

    }
}