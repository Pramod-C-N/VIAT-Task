using System;
using System.Collections.Generic;
using System.Text;
using UblSharp;
using System.Xml;

namespace vita.UblSharp.Dtos
{
    public class XMLSignInput
    {
        public string xml { get; set; }

        public string xml_uuid { get; set; }

        public InvoiceType xmlModel { get; set; } = null;

        public string qrPath { get; set; }
        public string UUID { get; set; }
        public int irnno { get; set; }

        public string SellerName { get; set; }
        public string VatId { get; set; }
        public decimal VatAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string xmlPath { get; set; }

        public string pathToSave { get; set; }

        public int? tenantId { get; set; }

        public bool isPhase1 { get; set; } = true;
    }

    public class XMLLogInput
    {
        public string uuid { get; set; }
        public int? tenantId { get; set; }
        public string signature { get; set; }
        public string certificate { get; set; }
        public string xml64 { get; set; }
        public string invoiceHash64 { get; set; }
        public string csid { get; set; }
        public string qrBase64 { get; set; }
        public string complianceInvoiceResponse { get; set; }
        public string reportInvoiceResponse { get; set; }
        public string clearanceResponse { get; set; }
        public decimal totalAmount { get; set; }
        public decimal vatAmount { get; set; }
        public string errors { get; set; }
        public int irnno { get; set; }
        public string status { get; set; }
    }

    public class VatCode
    {
        public string code { get; set; }
    }


}
