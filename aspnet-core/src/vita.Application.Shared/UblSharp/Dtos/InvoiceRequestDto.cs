using Abp.Application.Services.Dto;
using System;

namespace vita.UblSharp.Dtos
{
    public class InvoiceRequestDto: EntityDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }

        public string UUID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DateOfSupply { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyCodeOrignatingCountry { get; set; }
        public string PurchaseOrderId { get; set; }
        public string BillingRefrenceId { get; set; }
        public string ContractId { get; set; }


        public string TransactionType { get; set; }

        //public BuyerDto Buyer { get; set; }

        //public SupplierDto Supplier { get; set; }

        public DateTime LatestDeliveryDate { get; set; }
        public string Notes { get; set; }
       // public List<LineItem> LineItems { get; set; }
       // public List<Discount> Discount { get; set; }
        //public List<VatDetail> VatDetails { get; set; }
      //  public TotalDetails TotalDetails { get; set; }
      //  public List<PaymentDetail> PaymentDetails { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
        public string AdditionalInfo { get; set; }
        public string PaymentType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public string BranchCode { get; set; }
        public bool Nominal { get; set; }
        public bool Export { get; set; }
        public bool Summary { get; set; }
        public bool ThirdParty { get; set; }

        public string InvoiceTransactionType { get; set; }
        public string XmlHash { get; set; }
        public string PdfHash { get; set; }
    }
}
