using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Vendor
{
    [Table("VendorDocuments")]
    [Audited]
    public class VendorDocuments : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VendorID { get; set; }

        public virtual Guid VendorUniqueIdentifier { get; set; }

        public virtual string DocumentTypeCode { get; set; }

        public virtual string DocumentName { get; set; }

        public virtual string DocumentNumber { get; set; }

        public virtual DateTime DoumentDate { get; set; }

        public virtual string Status { get; set; }

    }
}