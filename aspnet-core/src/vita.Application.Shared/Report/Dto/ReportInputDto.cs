using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Report.Dto
{
    public class ReportInputDto
    {
        public DateTime Fromdate { get; set; }

        public DateTime Todate { get; set; }

        public string code { get; set; }
    }
}
