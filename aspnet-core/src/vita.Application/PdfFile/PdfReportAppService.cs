using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using Abp.UI;
using AutoMapper;
using iText.Kernel.Pdf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
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
using System.Transactions;
using System.Xml;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.EInvoicing.Dto;
using vita.EntityFrameworkCore;
using vita.MultiTenancy;
using vita.Sales.Dtos;
using vita.Storage;
using vita.Utils;

namespace vita.PdfFile
{
    public class PdfReportAppService : vitaAppServiceBase, IPdfReportAppService
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly TenantManager _tenantManager;
        private IDbContextProvider<vitaDbContext> _dbContextProvider;
        private IMapper mapper;


        public PdfReportAppService(IBinaryObjectManager binaryObjectManager, TenantManager tenantManager, IDbContextProvider<vitaDbContext> dbContextProvider,IMapper mapper)
        {
            _binaryObjectManager = binaryObjectManager;
            _tenantManager = tenantManager;
            _dbContextProvider = dbContextProvider;
            this.mapper = mapper;
        }

        public class PdfConf
        {
            public string html { get; set; }
            public string orientation { get; set; }
        }
        private async Task<PdfConf> GenerateHtml(int tenantId, string reportName, string json,bool isDraft)
        {
            var pdfConf = new PdfConf();
            try
            {
                string connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
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
                        cmd.Parameters.AddWithValue("@isqrCode", isDraft);
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
        private string GetInvoiceName(InvoiceTypeEnum code)
        {
            if (code == InvoiceTypeEnum.Sales)
            {
                return "Sales";
            }
            else if (code == InvoiceTypeEnum.Credit)
            {
                return "Credit";
            }
            else if (code == InvoiceTypeEnum.Debit)
            {
                return "Debit";
            }
            else return null;
        }

        public async Task<InvoiceResponse> GeneratePdfRequest<T>(T inputGen, string invoiceno, string uniqueIdentifier, string tenantId, InvoiceTypeEnum invoiceType,bool isDraft=false)
        
        {
            PDFRequest input = new PDFRequest();
            InvoiceResponse response = new();
           System.Diagnostics.Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(inputGen));
            try
            {
                input = mapper.Map<PDFRequest>(inputGen);
                var pdffileName = input.Supplier[0].VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".pdf";

                var imgPath = string.Empty;
            if (tenantId != null && tenantId != "")
                imgPath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png"; 
            else
                imgPath = @"wwwroot\InvoiceFiles\0\" + "\\" + uniqueIdentifier + "\\" + uniqueIdentifier + "_" + invoiceno + ".png";
            response.QRCodeUrl = imgPath;
            var logo = "";
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                logo = await BinaryToImage(tenantId);
                await unitOfWork.CompleteAsync();
            }
            var pdfConf = new PdfConf();
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                input.InvoiceNumber = invoiceno;
                pdfConf = await GenerateHtml((Convert.ToInt32(tenantId)), GetInvoiceName(invoiceType), JsonConvert.SerializeObject(input),isDraft);
                await unitOfWork.CompleteAsync();
            }
            pdfConf.html = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(pdfConf.html));
            pdfConf.html = pdfConf.html.Replace("@qrCode", imgPath);
            pdfConf.html = pdfConf.html.Replace("@logo", logo);
            var htmlpath = "";
            if (tenantId != null && tenantId != "")
                htmlpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\design.html";
            else
                htmlpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\design.html";
            File.WriteAllText(htmlpath, pdfConf.html);

         
                var pdfFullpath = string.Empty;
                if (tenantId != null && tenantId != "")
                    pdfFullpath = @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + pdffileName;
                else
                    pdfFullpath = @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + pdffileName;
                response.PdfFileUrl = pdfFullpath;
                var invoiceXml = string.Empty;
                var xmlfileName = input.Supplier[0].VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";

                if (!isDraft)
                {
                    if (tenantId != null && tenantId != "")
                        invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + xmlfileName);
                    else
                        invoiceXml = File.ReadAllText(@"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + xmlfileName);
                }
                response.XmlFileUrl = tenantId != null && tenantId != ""? @"wwwroot\InvoiceFiles\" + tenantId + "\\" + uniqueIdentifier + "\\" + xmlfileName: @"wwwroot\InvoiceFiles\0\" + uniqueIdentifier + "\\" + xmlfileName;
                PdfA3Generator.CreatePdf(Encoding.UTF8.GetBytes(pdfConf.html), Encoding.UTF8.GetBytes(invoiceXml), "", pdfFullpath, invoiceno, pdfConf.orientation,isDraft);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Error Generating PDF");
            }
            return response;
        }

     
        public async Task<string> BinaryToImage(string tenantId)
        {

            var logoPath = "";
                var tenant = await _tenantManager.FindByIdAsync(Convert.ToInt32(tenantId));
                if (tenantId != null && tenantId != "" && tenant.HasLogo())
                {
                    var logoObject = await _binaryObjectManager.GetOrNullAsync(tenant.LogoId.Value);
                    if (logoObject != null)
                    {
                        logoPath = "data:image/png;base64," + Convert.ToBase64String(logoObject.Bytes);
                    }
                else
                {
                    logoPath = @"wwwroot\InvoiceFiles" + "\\" + "Logo" + "\\" + "logo" + ".png";
                }
            }
                else
                {
                    logoPath = @"wwwroot\InvoiceFiles" + "\\" + "Logo" + "\\" + "logo" + ".png";
                }
            return logoPath;
        }

    }
}