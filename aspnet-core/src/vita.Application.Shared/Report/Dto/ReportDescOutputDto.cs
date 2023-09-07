using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Report.Dto
{

    public class VatCalculationReportDto
    {
        public int? SlNo { get; set; }
        public int? InvoiceNumber { get; set; }
        public string IssueDate { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}

