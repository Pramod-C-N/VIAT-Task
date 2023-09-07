using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Vendor
{
    [Table("VendorContactPerson")]
    [Audited]
    public class VendorContactPerson : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VendorID { get; set; }

        public virtual Guid VendorUniqueIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string EmployeeCode { get; set; }

        public virtual string ContactNumber { get; set; }

        public virtual string GovtId { get; set; }

        public virtual string Email { get; set; }

        public virtual string Address { get; set; }

        public virtual string Location { get; set; }

        public virtual string Type { get; set; }

    }
}