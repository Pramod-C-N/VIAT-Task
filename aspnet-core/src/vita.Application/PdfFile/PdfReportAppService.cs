using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using iText.Kernel.Pdf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.EntityFrameworkCore;
using vita.MultiTenancy;
using vita.Sales.Dtos;
using vita.Storage;
using vita.Utils;

namespace vita.PdfFile
{
    public class PdfReportAppService : IPdfReportAppService
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly TenantManager _tenantManager;
        private IDbContextProvider<vitaDbContext> _dbContextProvider;


        public PdfReportAppService(IBinaryObjectManager binaryObjectManager, TenantManager tenantManager, IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _binaryObjectManager = binaryObjectManager;
            _tenantManager = tenantManager;
            _dbContextProvider = dbContextProvider;
        }

        public class PdfConf
        {
            public string html { get; set; }
            public string orientation { get; set; }
        }
        public async Task<PdfConf> GenerateHtml(int tenantId, string reportName, string json)
        {
            var pdfConf = new PdfConf();
            try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GenerateHtml";
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@reportName", reportName);
                        cmd.Parameters.AddWithValue("@json", json);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            pdfConf.html = reader.GetString(0);
                            pdfConf.orientation = reader.GetString(1);
                            break;
                        }
                        conn.Close();

                        return pdfConf;
                    }
                }
            }
            catch (Exception e)
            {
                return pdfConf;
            }
        }

        public async Task<InvoiceResponse> GetPDFFile_Invoice(CreateOrEditSalesInvoiceDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
      

            var pdfConf = await GenerateHtml((Convert.ToInt32(tenantId)), "Sales", JsonConvert.SerializeObject(input));
            var imgPath = string.Empty;
            if (tenantId != null && tenantId != "")
                imgPath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png"; 
            else
                imgPath = @"wwwroot\InvoiceFiles\0\" + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png";

            pdfConf.html = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(pdfConf.html));

            var htmlpath = "";
            if (tenantId != null && tenantId != "")
                htmlpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\design.html";
            else
                htmlpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\design.html";
            File.WriteAllText(htmlpath, pdfConf.html);

            pdfConf.html = pdfConf.html.Replace("@qrCode", imgPath);
            pdfConf.html = pdfConf.html.Replace("@logo", await BinaryToImage(tenantId));

            try
            {
                var pdfName = uniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                var pdfFullpath = string.Empty;
                if (tenantId != null && tenantId != "")
                    pdfFullpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + pdfName;
                else
                    pdfFullpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + pdfName;

                var invoiceXml = string.Empty;
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";

                if (tenantId != null && tenantId != "")
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + xmlfileName);
                else
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + xmlfileName);

                PdfA3Generator.CreatePdf(Encoding.UTF8.GetBytes(pdfConf.html), Encoding.UTF8.GetBytes(invoiceXml), "", pdfFullpath, invoiceno, pdfConf.orientation);
            }
            catch (Exception e)
            {
                throw e;
            }
            InvoiceResponse response = new InvoiceResponse();
            return response;
        }

        public async Task<InvoiceResponse> GetPDFFile_CreditNote(CreateOrEditCreditNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
            var pdfConf = await GenerateHtml((Convert.ToInt32(tenantId)), "Credit", JsonConvert.SerializeObject(input));
            var imgPath = string.Empty;
            if (tenantId != null && tenantId != "")
                imgPath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png"; 
            else
                imgPath = @"wwwroot\InvoiceFiles\0\" + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png";

            pdfConf.html = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(pdfConf.html));

            var htmlpath = "";
            if (tenantId != null && tenantId != "")
                htmlpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\design.html";
            else
                htmlpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\design.html";
            File.WriteAllText(htmlpath, pdfConf.html);

            pdfConf.html = pdfConf.html.Replace("@qrCode", imgPath);
            pdfConf.html = pdfConf.html.Replace("@logo", await BinaryToImage(tenantId));

            try
            {
                var pdfName = uniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                var pdfFullpath = string.Empty;
                if (tenantId != null && tenantId != "")
                    pdfFullpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + pdfName;
                else
                    pdfFullpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + pdfName;

                var invoiceXml = string.Empty;
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";

                if (tenantId != null && tenantId != "")
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + xmlfileName); 
                else
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + xmlfileName);

                PdfA3Generator.CreatePdf(Encoding.UTF8.GetBytes(pdfConf.html), Encoding.UTF8.GetBytes(invoiceXml), "", pdfFullpath, invoiceno, pdfConf.orientation);
            }
            catch (Exception e)
            {
                throw e;
            }
            InvoiceResponse response = new InvoiceResponse();
            return response;
        }

        public async Task<InvoiceResponse> GetPDFFile_DebitNote(CreateOrEditDebitNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId)
        {
            var pdfConf = await GenerateHtml((Convert.ToInt32(tenantId)), "Debit", JsonConvert.SerializeObject(input));
            var imgPath = string.Empty;
            if (tenantId != null && tenantId != "")
                imgPath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png"; 
            else
                imgPath = @"wwwroot\InvoiceFiles\0\" + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png";

            pdfConf.html = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(pdfConf.html));

            var htmlpath = "";
            if (tenantId != null && tenantId != "")
                htmlpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\design.html";
            else
                htmlpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\design.html";
            File.WriteAllText(htmlpath, pdfConf.html);

            pdfConf.html = pdfConf.html.Replace("@qrCode", imgPath);
            pdfConf.html = pdfConf.html.Replace("@logo", await BinaryToImage(tenantId));

            try
            {
                var pdfName = uniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                var pdfFullpath = string.Empty;
                if (tenantId != null && tenantId != "")
                    pdfFullpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + pdfName;
                else
                    pdfFullpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + pdfName;

                var invoiceXml = string.Empty;
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";

                if (tenantId != null && tenantId != "")
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + xmlfileName); //File.ReadAllText(@"wwwroot\InvoiceFiles\" + response.Uuid + "\\" + response.Uuid + "_" + request.InvoiceNumber.ToString() + ".xml");
                else
                    invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + xmlfileName);

                PdfA3Generator.CreatePdf(Encoding.UTF8.GetBytes(pdfConf.html), Encoding.UTF8.GetBytes(invoiceXml), "", pdfFullpath, invoiceno,pdfConf.orientation);
            }
            catch (Exception e)
            {
                throw e;
            }
            InvoiceResponse response = new InvoiceResponse();
            return response;
        }

        public async Task<string> BinaryToImage(string tenantId)
        {
            var logoPath = "";
            var tenant = await _tenantManager.FindByIdAsync(Convert.ToInt32(tenantId));
            if (tenantId != null && tenantId != "" && tenant.HasLogo())
            {
                var logoObject = await _binaryObjectManager.GetOrNullAsync(tenant.LogoId.Value);
                logoPath = "data:image/png;base64," + Convert.ToBase64String(logoObject.Bytes);
            }
            else
            {
                logoPath = @"wwwroot\InvoiceFiles" + "\\" + "Logo" + "\\" + "logo" + ".png";
            }
            return logoPath;
        }

    }
}