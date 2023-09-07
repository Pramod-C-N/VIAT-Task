using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace vita.CustomReportSP
{
    [Table("CustomReport")]
    public class CustomReport : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string ReportName { get; set; }

        public virtual string StoredProcedureName { get; set; }

    }
}