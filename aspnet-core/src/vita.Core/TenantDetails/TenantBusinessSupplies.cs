using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantBusinessSupplies")]
    [Audited]
    public class TenantBusinessSupplies : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string BusinessTypeID { get; set; }

        public virtual string BusinessSupplies { get; set; }

        public virtual bool IsActive { get; set; }

    }
}