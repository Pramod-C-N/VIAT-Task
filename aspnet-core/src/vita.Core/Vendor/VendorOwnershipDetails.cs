using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Vendor
{
    [Table("VendorOwnershipDetails")]
    [Audited]
    public class VendorOwnershipDetails : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string VendorID { get; set; }

        public virtual Guid VendorUniqueIdentifier { get; set; }

        public virtual string PartnerName { get; set; }

        public virtual string PartnerConstitution { get; set; }

        public virtual string PartnerNationality { get; set; }

        public virtual decimal CapitalAmount { get; set; }

        public virtual decimal CapitalShare { get; set; }

        public virtual decimal ProfitShare { get; set; }

        public virtual string RepresentativeName { get; set; }

    }
}