using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Vendor
{
    [Table("VendorSectorDetail")]
    [Audited]
    public class VendorSectorDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VendorID { get; set; }

        public virtual Guid VendorUniqueIdentifier { get; set; }

        public virtual string SubIndustryCode { get; set; }

        public virtual string SubIndustryName { get; set; }

    }
}