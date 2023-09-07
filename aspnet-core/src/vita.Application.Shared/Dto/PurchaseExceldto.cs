using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Dto
{
    public class PurchaseExcelDto
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string VAT { get; set; }
        public string Address { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }
        public string Type { get; set; }
    }
}
