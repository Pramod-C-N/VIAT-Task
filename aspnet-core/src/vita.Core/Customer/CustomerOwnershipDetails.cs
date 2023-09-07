using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Customer
{
    [Table("CustomerOwnershipDetails")]
    [Audited]
    public class CustomerOwnershipDetails : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string CustomerID { get; set; }

        public virtual Guid CustomerUniqueIdentifier { get; set; }

        public virtual string PartnerName { get; set; }

        public virtual string PartnerConstitution { get; set; }

        public virtual string PartnerNationality { get; set; }

        public virtual decimal CapitalAmount { get; set; }

        public virtual decimal CapitalShare { get; set; }

        public virtual decimal ProfitShare { get; set; }

        public virtual string RepresentativeName { get; set; }

    }
}