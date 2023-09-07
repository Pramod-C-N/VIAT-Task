using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Debit
{
    [Table("DebitNote")]
    [Audited]
    public class DebitNote : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        [Required]
        public virtual string IRNNo { get; set; }

        public virtual string InvoiceNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual DateTime DateOfSupply { get; set; }

        public virtual string InvoiceCurrencyCode { get; set; }

        public virtual string CurrencyCodeOriginatingCountry { get; set; }

        public virtual string PurchaseOrderId { get; set; }

        public virtual string BillingReferenceId { get; set; }

        public virtual string ContractId { get; set; }

        public virtual DateTime LatestDeliveryDate { get; set; }

        public virtual string Location { get; set; }

        public virtual string CustomerId { get; set; }

        public virtual string Status { get; set; }

        public virtual string Additional_Info { get; set; }

        public virtual string PaymentType { get; set; }

        public virtual string PdfUrl { get; set; }

        public virtual string QrCodeUrl { get; set; }

        public virtual string XMLUrl { get; set; }

        public virtual string ArchivalUrl { get; set; }

        public virtual string PreviousInvoiceHash { get; set; }

        public virtual string PerviousXMLHash { get; set; }

        public virtual string XMLHash { get; set; }

        public virtual string PdfHash { get; set; }

        public virtual string XMLbase64 { get; set; }

        public virtual string PdfBase64 { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual int TransTypeCode { get; set; }

        public virtual string TransTypeDescription { get; set; }

        public virtual string AdvanceReferenceNumber { get; set; }

        public virtual string Invoicetransactioncode { get; set; }

        public virtual string BusinessProcessType { get; set; }

        public virtual string InvoiceNotes { get; set; }

    }
}