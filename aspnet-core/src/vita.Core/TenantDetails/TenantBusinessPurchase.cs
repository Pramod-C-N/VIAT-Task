using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantBusinessPurchase")]
    [Audited]
    public class TenantBusinessPurchase : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string BusinessTypeID { get; set; }

        public virtual string BusinessPurchase { get; set; }

        public virtual bool IsActive { get; set; }

    }
}