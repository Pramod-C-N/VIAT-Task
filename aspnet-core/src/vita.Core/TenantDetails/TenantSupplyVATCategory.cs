using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantSupplyVATCategory")]
    [Audited]
    public class TenantSupplyVATCategory : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VATCategoryID { get; set; }

        public virtual string VATCategoryName { get; set; }

        public virtual string VATCode { get; set; }

        public virtual bool IsActive { get; set; }

    }
}