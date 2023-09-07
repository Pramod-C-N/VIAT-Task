using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Sales.Dtos
{
    public class InvoiceResponse
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        //public int SequenceNumber { get; set; }
        public Guid Uuid { get; set; }
        //public string TypeCode { get; set; }
        //public string TransactionCode { get; set; }
        //public string PreviousHash { get; set; }
        //public string QRCode { get; set; }
        public string QRCodeUrl { get; set; }
        public string PdfFileUrl { get; set; }
        public string XmlFileUrl { get; set; }
    }
}
