using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantDocuments")]
    [Audited]
    public class TenantDocuments : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string BranchId { get; set; }

        public virtual string BranchName { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string DocumentId { get; set; }

        public virtual string DocumentNumber { get; set; }

        public virtual DateTime? RegistrationDate { get; set; }

        public virtual string DocumentPath { get; set; }

    }
}