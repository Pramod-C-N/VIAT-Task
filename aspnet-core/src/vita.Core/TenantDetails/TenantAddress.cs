using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantAddress")]
    [Audited]
    public class TenantAddress : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string AddressTypeId { get; set; }

        public virtual string AddressType { get; set; }

        public virtual string BuildingNo { get; set; }

        public virtual string AdditionalBuildingNumber { get; set; }

        public virtual string Street { get; set; }

        public virtual string AdditionalStreet { get; set; }

        public virtual string Neighbourhood { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string Country { get; set; }

        public virtual string CountryCode { get; set; }

    }
}