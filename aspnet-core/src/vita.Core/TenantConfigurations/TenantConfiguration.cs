using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace vita.TenantConfigurations
{
    [Table("TenantConfiguration")]
    public class TenantConfiguration : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool isPhase1 { get; set; }

        public virtual string TransactionType { get; set; }

        public virtual string ShipmentJson { get; set; }

        public virtual string AdditionalFieldsJson { get; set; }

        public virtual string EmailJson { get; set; }

        public virtual string AdditionalData1 { get; set; }

        public virtual string AdditionalData2 { get; set; }

        public virtual string AdditionalData3 { get; set; }

        public virtual bool isActive { get; set; }

        public virtual string Language { get; set; }

    }
}