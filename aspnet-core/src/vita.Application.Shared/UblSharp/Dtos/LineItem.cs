using System;
using System.Collections.Generic;
using System.Text;

namespace vita.UblSharp.Dtos
{
    public class LineItem
    {
        public string InvoiceNumber { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BuyerIdentifier { get; set; }
        public string SellerIdentifier { get; set; }
        public string StandardIdentifier { get; set; }
        public string Uom { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal VatPrice { get; set; }
        public decimal VatRate { get; set; }
        public string VatCode { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineAmountInclusiveVat { get; set; }
        public string CurrencyCode { get; set; }
        public string TaxSchemeId { get; set; }
        public string Notes { get; set; }
        public string BranchCode { get; set; }

    }
}
