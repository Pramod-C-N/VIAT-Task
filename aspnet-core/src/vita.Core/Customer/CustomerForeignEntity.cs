using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerForeignEntity")]
    [Audited]
    public class CustomerForeignEntity : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

        public virtual string ForeignEntityName { get; set; }

        public virtual string ForeignEntityAddress { get; set; }

        public virtual string LegalRepresentative { get; set; }

        public virtual string Country { get; set; }

    }
}