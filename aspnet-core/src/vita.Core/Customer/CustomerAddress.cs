using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerAddress")]
    [Audited]
    public class CustomerAddress : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

        public virtual string Street { get; set; }

        public virtual string AdditionalStreet { get; set; }

        public virtual string BuildingNo { get; set; }

        public virtual string AdditionalNo { get; set; }

        public virtual string City { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string State { get; set; }

        public virtual string Neighbourhood { get; set; }

        public virtual string CountryCode { get; set; }

        public virtual string Type { get; set; }

    }
}