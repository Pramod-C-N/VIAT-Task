using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("BatchData")]
    [Audited]
    public class BatchData : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long BatchId { get; set; }

        public virtual string FileName { get; set; }

        public virtual int TotalRecords { get; set; }

        public virtual int? SuccessRecords { get; set; }

        public virtual int? FailedRecords { get; set; }

        public virtual string Status { get; set; }

        public virtual string FilePath { get; set; }

        public virtual string DataPath { get; set; }

        public virtual string Type { get; set; }

        public virtual DateTime? fromDate { get; set; }

        public virtual DateTime? toDate { get; set; }

    }
}