using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerDocuments")]
    [Audited]
    public class CustomerDocuments : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

        public virtual string DocumentTypeCode { get; set; }

        public virtual string DocumentName { get; set; }

        public virtual string DocumentNumber { get; set; }

        public virtual DateTime DoumentDate { get; set; }

        public virtual string Status { get; set; }

    }
}