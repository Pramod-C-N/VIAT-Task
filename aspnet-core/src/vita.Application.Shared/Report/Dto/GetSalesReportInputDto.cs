using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Report.Dto
{
    public class GetSalesReportInputDto
    {
        public DateTime Fromdate { get; set; }

        public DateTime Todate { get; set; }

        public string code { get; set; }
        public string subcode { get; set; } = null;

        public string text { get; set; } = null;

        public string type { get; set; } = null;

    }
}
