using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("Activecurrency")]
    [Audited]
    public class Activecurrency : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string Entity { get; set; }

        public virtual string Currency { get; set; }

        public virtual string AlphabeticCode { get; set; }

        public virtual string NumericCode { get; set; }

        public virtual string MinorUnit { get; set; }

        public virtual bool IsActive { get; set; }

    }
}