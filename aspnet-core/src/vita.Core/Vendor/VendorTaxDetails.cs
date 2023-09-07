using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Vendor
{
    [Table("VendorTaxDetails")]
    [Audited]
    public class VendorTaxDetails : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VendorID { get; set; }

        public virtual Guid VendorUniqueIdentifier { get; set; }

        public virtual string BusinessCategory { get; set; }

        public virtual string OperatingModel { get; set; }

        public virtual string BusinessSupplies { get; set; }

        public virtual string SalesVATCategory { get; set; }

        public virtual string InvoiceType { get; set; }

    }
}