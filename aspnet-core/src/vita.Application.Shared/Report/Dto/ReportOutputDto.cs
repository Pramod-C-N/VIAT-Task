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

    public class OverHeadGapReportDto
    {
        public int Id { get; set; }
        public string particulars { get; set; }
        public decimal? col1 { get; set; }
        public decimal? col2 { get; set; }
        public decimal? col3 { get; set; }
        public decimal? col4 { get; set; }
        public decimal? col5 { get; set; }
        public decimal? col6 { get; set; }
        public decimal? col7 { get; set; }
        public decimal? col8 { get; set; }
        public decimal? col9 { get; set; }
        public decimal? col10 { get; set; }
        public decimal? col11 { get; set; }
        public decimal? col12 { get; set; }
        public decimal? Amount { get; set; }
        public int style { get; set; }


    }
}

