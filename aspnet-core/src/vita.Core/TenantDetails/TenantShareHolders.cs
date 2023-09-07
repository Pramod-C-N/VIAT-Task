using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantShareHolders")]
    [Audited]
    public class TenantShareHolders : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string PartnerName { get; set; }

        public virtual string Designation { get; set; }

        public virtual string Nationality { get; set; }

        public virtual string CapitalAmount { get; set; }

        public virtual string CapitalShare { get; set; }

        public virtual string ProfitShare { get; set; }

        public virtual string ConstitutionName { get; set; }

        public virtual string RepresentativeName { get; set; }

        public virtual string DomesticName { get; set; }

    }
}