using System;
using System.Collections.Generic;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;
using System.Drawing;

namespace vita.Utils
{
    public class QRCodeGeneration
    {
        byte[] Seller;
        byte[] VatNo;
        byte[] dateTime;
        byte[] Total;
        byte[] Tax;
        //byte[] XmlHash;
        //byte[] PdfHash;

        public QRCodeGeneration(String Seller, String TaxNo, DateTime dateTime, Double Total, Double Tax)
        {
            this.Seller = Encoding.UTF8.GetBytes(Seller);
            this.VatNo = Encoding.UTF8.GetBytes(TaxNo);

            this.dateTime = Encoding.UTF8.GetBytes(dateTime.ToString());
            this.Total = Encoding.UTF8.GetBytes(Total.ToString());
            this.Tax = Encoding.UTF8.GetBytes(Tax.ToString());
        }

        public QRCodeGeneration(String Seller, String TaxNo, DateTime dateTime, Double Total, Double Tax, string xmlHash, string pdfHash)
        {
            this.Seller = Encoding.UTF8.GetBytes(Seller);
            this.VatNo = Encoding.UTF8.GetBytes(TaxNo);

            this.dateTime = Encoding.UTF8.GetBytes(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            this.Total = Encoding.UTF8.GetBytes(Total.ToString());
            this.Tax = Encoding.UTF8.GetBytes(Tax.ToString());

            //this.XmlHash = Encoding.UTF8.GetBytes(xmlHash);
            //this.PdfHash = Encoding.UTF8.GetBytes(pdfHash);
        }

        public Bitmap toQrCode(int height = 250, int width = 250)
        {
            try
            {
                //Bitmap bmp;
                BarcodeWriter barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Width = width,
                        Height = height
                    }
                };
                Bitmap QrCode = barcodeWriter.Write(this.ToBase64());

                return QrCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private String getasText(int Tag, byte[] Value)
        {
            return (Tag).ToString("X2") + (Value.Length).ToString("X2") + BitConverter.ToString(Value).Replace("-", string.Empty);
        }

        private byte[] getBytes(int id, byte[] Value)
        {
            byte[] val = new byte[2 + Value.Length];
            val[0] = (byte)id;
            val[1] = (byte)Value.Length;
            Value.CopyTo(val, 2);
            return val;
        }

        private String getString()
        {
            String TLV_Text = "";
            TLV_Text += this.getasText(1, this.Seller);
            TLV_Text += this.getasText(2, this.VatNo);
            TLV_Text += this.getasText(3, this.dateTime);
            TLV_Text += this.getasText(4, this.Total);
            TLV_Text += this.getasText(5, this.Tax);
            //if (this.XmlHash != null && this.XmlHash.Length > 0)
            //    TLV_Text += this.getasText(6, this.XmlHash);
            //if (this.PdfHash != null && this.PdfHash.Length > 0)
            //    TLV_Text += this.getasText(7, this.PdfHash);
            return TLV_Text;
        }

        public override string ToString()
        {
            return this.getString();
        }

        public String ToBase64()
        {
            List<byte> TLV_Bytes = new List<byte>();
            TLV_Bytes.AddRange(getBytes(1, this.Seller));
            TLV_Bytes.AddRange(getBytes(2, this.VatNo));
            TLV_Bytes.AddRange(getBytes(3, this.dateTime));
            TLV_Bytes.AddRange(getBytes(4, this.Total));
            TLV_Bytes.AddRange(getBytes(5, this.Tax));
            //if (this.XmlHash != null && this.XmlHash.Length > 0)
            //    TLV_Bytes.AddRange(getBytes(6, this.XmlHash));
            //if (this.PdfHash != null && this.PdfHash.Length > 0)
            //    TLV_Bytes.AddRange(getBytes(7, this.PdfHash));
            return Convert.ToBase64String(TLV_Bytes.ToArray());
        }
    }
}
