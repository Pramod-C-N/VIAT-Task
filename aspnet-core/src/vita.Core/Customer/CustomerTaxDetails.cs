using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerTaxDetails")]
    [Audited]
    public class CustomerTaxDetails : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

        public virtual string BusinessCategory { get; set; }

        public virtual string OperatingModel { get; set; }

        public virtual string BusinessSupplies { get; set; }

        public virtual string SalesVATCategory { get; set; }

        public virtual string InvoiceType { get; set; }

    }
}