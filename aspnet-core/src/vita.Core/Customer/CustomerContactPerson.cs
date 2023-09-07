using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerContactPerson")]
    [Audited]
    public class CustomerContactPerson : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

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