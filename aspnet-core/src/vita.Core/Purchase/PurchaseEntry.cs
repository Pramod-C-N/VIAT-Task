using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.Purchase
{
    [Table("PurchaseEntry")]
    [Audited]
    public class PurchaseEntry : FullAuditedEntity<long>, IMayHaveTenant
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

        public virtual string PurchaseNumber { get; set; }

        public virtual DateTime SupplierInvoiceDate { get; set; }

        public virtual string BillOfEntry { get; set; }

        public virtual DateTime BillOfEntryDate { get; set; }

        public virtual decimal CustomsPaid { get; set; }

        public virtual decimal CustomTax { get; set; }

        public virtual bool IsWHT { get; set; }

        public virtual bool VATDeffered { get; set; }

        public virtual string PlaceofSupply { get; set; }

    }
}