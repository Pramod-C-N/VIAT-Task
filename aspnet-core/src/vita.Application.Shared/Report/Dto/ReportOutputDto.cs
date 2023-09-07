using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Report.Dto
{

        public class VatReportDto
        {
            public int Id { get; set; }
            public string text { get; set; }
            public decimal? Amount { get; set; }
            public decimal? Adjustment { get; set; }
            public decimal? Vat { get; set; }
            public int style { get; set; }
    

    }
}

