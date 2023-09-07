using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Dto
{
    public class WHTExcelDto
    {
        public string Month { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }  

        public string ToMonth { get; set; } 
        public string Year { get; set; }
        public string FiscalYear { get; set; }
        public string WithholderName { get; set; }
        public string FinancialNumber { get; set; }

        public string Type { get; set; }
        public string code { get; set; }
    }
}
