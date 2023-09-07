using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("Module")]
    [Audited]
    public class Module : FullAuditedEntity<long>
    {

        public virtual int ModuleId { get; set; }

        [Required]
        public virtual string ModuleName { get; set; }

        public virtual int Status { get; set; }

        public virtual int? TenantId { get; set; }

    }
}