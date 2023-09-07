using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("IRNMaster")]
    [Audited]
    public class IRNMaster : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual long IRNNo { get; set; }

        public virtual string TransactionType { get; set; }

        public virtual string Status { get; set; }

    }
}