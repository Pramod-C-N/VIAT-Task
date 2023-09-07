using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Debit
{
    [Table("DebitNoteSummary")]
    [Audited]
    public class DebitNoteSummary : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual decimal NetInvoiceAmount { get; set; }

        public virtual string NetInvoiceAmountCurrency { get; set; }

        public virtual decimal SumOfInvoiceLineNetAmount { get; set; }

        public virtual string SumOfInvoiceLineNetAmountCurrency { get; set; }

        public virtual decimal TotalAmountWithoutVAT { get; set; }

        public virtual string TotalAmountWithoutVATCurrency { get; set; }

        public virtual decimal TotalVATAmount { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual decimal TotalAmountWithVAT { get; set; }

        public virtual decimal PaidAmount { get; set; }

        public virtual string PaidAmountCurrency { get; set; }

        public virtual decimal PayableAmount { get; set; }

        public virtual string PayableAmountCurrency { get; set; }

        public virtual decimal AdvanceAmountwithoutVat { get; set; }

        public virtual decimal AdvanceVat { get; set; }

        public virtual string AdditionalData1 { get; set; }

    }
}