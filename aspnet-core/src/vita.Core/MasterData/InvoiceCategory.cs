﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("InvoiceCategory")]
    [Audited]
    public class InvoiceCategory : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Code { get; set; }

        public virtual bool IsActive { get; set; }

    }
}