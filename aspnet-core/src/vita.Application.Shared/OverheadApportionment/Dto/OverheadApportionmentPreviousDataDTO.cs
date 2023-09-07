using System;
using System.Collections.Generic;
using System.Text;

namespace vita.OverheadApportionment.Dto
{
    public class OverheadApportionmentPreviousDataDTO
    {
        public decimal TaxableSupplies { get; set; }
        public decimal ExemptSupplies { get; set; }
        public decimal ExemptTaxableSupplies { get; set; }
        public decimal TaxablePurchase { get; set; }
        public decimal ExemptPurchase { get; set; }
        public decimal ExemptTaxablePurchase { get; set; }
        public decimal PercentageofTaxable { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
    }
}
