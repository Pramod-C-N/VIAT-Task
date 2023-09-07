using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace vita.TenantDetails
{
    [Table("TenantBankDetail")]
    public class TenantBankDetail : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string IBAN { get; set; }

        public virtual string BankName { get; set; }

        public virtual string SwiftCode { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string BranchName { get; set; }

        public virtual string BranchAddress { get; set; }

    }
}