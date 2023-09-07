using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("Customers")]
    [Audited]
    public class Customers : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string TenantType { get; set; }

        public virtual string ConstitutionType { get; set; }

        public virtual string Name { get; set; }

        public virtual string LegalName { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual string ContactNumber { get; set; }

        public virtual string EmailID { get; set; }

        public virtual string Nationality { get; set; }

        public virtual string Designation { get; set; }

    }
}