﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.DraftFee
{
    [Table("DraftContactPerson")]
    [Audited]
    public class DraftContactPerson : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual string Name { get; set; }

        public virtual string EmployeeCode { get; set; }

        public virtual string ContactNumber { get; set; }

        public virtual string GovtId { get; set; }

        public virtual string Email { get; set; }

        public virtual string Address { get; set; }

        public virtual string Location { get; set; }

        public virtual string Type { get; set; }

        public virtual string AdditionalData1 { get; set; }

        public virtual string Language { get; set; }

    }
}