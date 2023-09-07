using System;
using System.Drawing;
using System.IO;
using System.Linq;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.Sales.Dtos;

namespace vita.Utils
{
    public static class QrCodeProvider
    {
        public static string QrCodeGeneration(CreateOrEditSalesInvoiceDto input, string invoiceno)
        {
            try
            {
                var pathToSave = Path.Combine("wwwroot/InvoiceFiles", invoiceno); //Path.Combine("wwwroot/InvoiceFiles", response.Uuid.ToString());

                var tt1 = input.Items.Sum(a => (a.UnitPrice * a.Quantity)).ToString("0.##");
                var tt2 = input.Items.Sum(a => (a.VATAmount)).ToString("0.##");
                var tt3 = input.Items.Sum(a => (a.LineAmountInclusiveVAT)).ToString("0.##");

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var qrfileName = invoiceno + ".png"; //response.Uuid + "_" + request.InvoiceNumber.ToString() + ".png";
                var path = (Path.Combine(pathToSave, qrfileName));

                //Bitmap qrCodeImage = new QRCodeGeneration(request.Supplier.RegistrationName, request.Supplier.VatId.Replace("VAT:", ""),
                //                                    request.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), request.XmlHash, request.PdfHash).toQrCode();

                var qr = new QRCodeGeneration("Supplier", "V1",
                                        input.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), "xxxxxx", "ppppppp");
                Bitmap qrCodeImage = qr.toQrCode();


                qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return qr.ToBase64();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //response.QRCode = "";
        }

        public static string CreditNoteQrCodeGeneration(CreateOrEditCreditNoteDto input, string invoiceno)
        {
            try
            {
                var pathToSave = Path.Combine("wwwroot/InvoiceFiles", invoiceno); //Path.Combine("wwwroot/InvoiceFiles", response.Uuid.ToString());

                var tt1 = input.Items.Sum(a => (a.UnitPrice * a.Quantity)).ToString("0.##");
                var tt2 = input.Items.Sum(a => (a.VATAmount)).ToString("0.##");
                var tt3 = input.Items.Sum(a => (a.LineAmountInclusiveVAT)).ToString("0.##");

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var qrfileName = invoiceno + ".png"; //response.Uuid + "_" + request.InvoiceNumber.ToString() + ".png";
                var path = (Path.Combine(pathToSave, qrfileName));

                //Bitmap qrCodeImage = new QRCodeGeneration(request.Supplier.RegistrationName, request.Supplier.VatId.Replace("VAT:", ""),
                //                                    request.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), request.XmlHash, request.PdfHash).toQrCode();
                var qr = new QRCodeGeneration("Supplier", "V1",
                                       input.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), "xxxxxx", "ppppppp");
                Bitmap qrCodeImage = qr.toQrCode();


                qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return qr.ToBase64();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //response.QRCode = "";
        }

        public static string QrCodeGeneration_Invoice(CreateOrEditSalesInvoiceDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
            try
            {
                string pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);

                var tt1 = input.Items.Sum(a => (a.UnitPrice * a.Quantity)).ToString("0.##");
                var tt2 = input.Items.Sum(a => (a.VATAmount)).ToString("0.##");
                var tt3 = input.Items.Sum(a => (a.LineAmountInclusiveVAT)).ToString("0.##");

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var qrfileName = uniqueIdentifier + "_" + invoiceno + ".png"; //response.Uuid + "_" + request.InvoiceNumber.ToString() + ".png";
                var path = (Path.Combine(pathToSave, qrfileName));

                //Bitmap qrCodeImage = new QRCodeGeneration(request.Supplier.RegistrationName, request.Supplier.VatId.Replace("VAT:", ""),
                //                                    request.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), request.XmlHash, request.PdfHash).toQrCode();

                var qr = new QRCodeGeneration(input.Supplier.RegistrationName, input.Supplier.VATID,
                                       input.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), "xxxxxx", "ppppppp");
                Bitmap qrCodeImage = qr.toQrCode();


                qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return qr.ToBase64();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //response.QRCode = "";
        }


        public static string QrCodeGeneration_CreditNote(CreateOrEditCreditNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
            try
            {
                string pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);

                var tt1 = input.Items.Sum(a => (a.UnitPrice * a.Quantity)).ToString("0.##");
                var tt2 = input.Items.Sum(a => (a.VATAmount)).ToString("0.##");
                var tt3 = input.Items.Sum(a => (a.LineAmountInclusiveVAT)).ToString("0.##");

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var qrfileName = uniqueIdentifier + "_" + invoiceno + ".png"; //response.Uuid + "_" + request.InvoiceNumber.ToString() + ".png";
                var path = (Path.Combine(pathToSave, qrfileName));

                //Bitmap qrCodeImage = new QRCodeGeneration(request.Supplier.RegistrationName, request.Supplier.VatId.Replace("VAT:", ""),
                //                                    request.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), request.XmlHash, request.PdfHash).toQrCode();

                var qr = new QRCodeGeneration(input.Supplier.RegistrationName, input.Supplier.VATID,
                                        input.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), "xxxxxx", "ppppppp");
                Bitmap qrCodeImage = qr.toQrCode();


                qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return qr.ToBase64();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //response.QRCode = "";
        }

        public static string QrCodeGeneration_DebitNote(CreateOrEditDebitNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
            try
            {
                string pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);

                var tt1 = input.Items.Sum(a => (a.UnitPrice * a.Quantity)).ToString("0.##");
                var tt2 = input.Items.Sum(a => (a.VATAmount)).ToString("0.##");
                var tt3 = input.Items.Sum(a => (a.LineAmountInclusiveVAT)).ToString("0.##");

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var qrfileName = uniqueIdentifier + "_" + invoiceno + ".png"; //response.Uuid + "_" + request.InvoiceNumber.ToString() + ".png";
                var path = (Path.Combine(pathToSave, qrfileName));

                //Bitmap qrCodeImage = new QRCodeGeneration(request.Supplier.RegistrationName, request.Supplier.VatId.Replace("VAT:", ""),
                //                                    request.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), request.XmlHash, request.PdfHash).toQrCode();

                var qr = new QRCodeGeneration(input.Supplier.RegistrationName, input.Supplier.VATID,
                                        input.IssueDate, Convert.ToDouble(tt3), Convert.ToDouble(tt2), "xxxxxx", "ppppppp");
                Bitmap qrCodeImage = qr.toQrCode();


                qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return qr.ToBase64();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //response.QRCode = "";
        }


    }
}