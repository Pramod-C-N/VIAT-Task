using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using vita.MasterData;
using vita.MasterData.Dtos;
using System.IO;
using vita.Utils;
using vita.UblSharp;
using vita.PdfFile;
using System.Data;
using Microsoft.Data.SqlClient;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using vita.TenantDetails;
using Z.EntityFramework.Plus;
using System.Transactions;
using Abp.Timing.Timezone;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using vita.Filters;
using static vita.Filters.VitaFilter_Validation;
using DbDataReaderMapper;
using System.Linq.Expressions;
using System.DirectoryServices.Protocols;
using AutoMapper;
using vita.EInvoicing.Dto;
using vita.MultiTenancy.Accounting.Dto;
using Microsoft.AspNetCore.Authorization;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using vita.TenantConfigurations.Dtos;
using vita.TenantConfigurations;

namespace vita.Sales
{
    public class SalesInvoicesDomainService : DomainService, ISalesInvoicesDomainService
    {
        private readonly IRepository<SalesInvoice, long> _salesInvoiceRepository;
        private readonly ISalesInvoicePartiesAppService _partyAppService;
        private readonly ISalesInvoiceItemsAppService _invoiceItemsAppService;
        private readonly ISalesInvoiceVATDetailsAppService _vatDetailsAppService;
        private readonly ISalesInvoicePaymentDetailsAppService _paymentDetailsAppService;
        private readonly ISalesInvoiceSummariesAppService _invoiceSummariesAppService;
        private readonly ISalesInvoiceDiscountsAppService _discountAppService;
        private readonly ISalesInvoiceContactPersonsAppService _contactPersonsAppService;
        private readonly ISalesInvoiceAddressesAppService _invoiceAddressesAppService;
        private readonly IIRNMastersAppService _transactionsAppService;
        private readonly IGenerateXmlAppService _generateXmlAppService;
        private readonly IPdfReportAppService _pdfReportAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITenantBasicDetailsAppService _tenantbasicdetails;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IMapper mapper;
        private string connStr = "";

        public SalesInvoicesDomainService(IRepository<SalesInvoice, long> salesInvoiceRepository,
            ISalesInvoicePartiesAppService partyAppService,
       ISalesInvoiceItemsAppService invoiceItemsAppService,
       ISalesInvoiceVATDetailsAppService vatDetailsAppService,
       ISalesInvoicePaymentDetailsAppService paymentDetailsAppService,
       ISalesInvoiceSummariesAppService invoiceSummariesAppService,
       ISalesInvoiceDiscountsAppService discountAppService,
       ISalesInvoiceContactPersonsAppService contactPersonsAppService,
       ISalesInvoiceAddressesAppService invoiceAddressesAppService,
       IIRNMastersAppService transactionsAppService,
       IGenerateXmlAppService generateXmlAppService,
       IPdfReportAppService pdfReportAppService,
       ITenantBasicDetailsAppService tenantbasicdetails,
       IDbContextProvider<vitaDbContext> dbContextProvider,
       ITimeZoneConverter timeZoneConverter,
       IMapper mapper)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
            _contactPersonsAppService = contactPersonsAppService;
            _discountAppService = discountAppService;
            _generateXmlAppService = generateXmlAppService;
            _invoiceAddressesAppService = invoiceAddressesAppService;
            _invoiceSummariesAppService = invoiceSummariesAppService;
            _invoiceItemsAppService = invoiceItemsAppService;
            _partyAppService = partyAppService;
            _vatDetailsAppService = vatDetailsAppService;
            _paymentDetailsAppService = paymentDetailsAppService;
            _transactionsAppService = transactionsAppService;
            _pdfReportAppService = pdfReportAppService;
            _dbContextProvider = dbContextProvider;
            _tenantbasicdetails = tenantbasicdetails;
            _timeZoneConverter = timeZoneConverter;
            this.mapper = mapper;
        }

        public async Task<bool> UpdateInvoiceURL(InvoiceResponse response,int tenantId, string type)
        {

            try
            {
                var connStr1 = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr1))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "UpdateInvoiceURL";

                        cmd.Parameters.AddWithValue("@pdfurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@xmlurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@qrurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@irn", response.InvoiceId.ToString());
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@type", type);


                        int i = cmd.ExecuteNonQuery();

                        conn.Close();

                        return i > 0;

                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<InvoiceResponse> GetSalesInvoiceData(int? irnNo, int tenantId, string transType)
        {
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(tenantId);

            InvoiceResponse pdfResponse = new();
            IRNMaster irn = new();
            CreateOrEditSalesInvoiceDto data = new();
            CreateOrEditSalesInvoicePartyDto supplier = new();
            supplier.Address = new();
            supplier.ContactPerson = new();
            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                supplier.RegistrationName = row["TenancyName"].ToString();

                supplier.VATID = row["vatid"].ToString();
                supplier.Website = row["website"].ToString();
                supplier.FaxNo = row["faxNo"].ToString();
                //Address
                supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                supplier.Address.BuildingNo = row["BuildingNo"].ToString();
                supplier.Address.Street = row["Street"].ToString();
                supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                supplier.Address.PostalCode = row["PostalCode"].ToString();
                supplier.Address.CountryCode = row["country"].ToString();
                supplier.Address.City = row["city"].ToString();
                supplier.Address.State = row["State"].ToString();
                supplier.CRNumber = row["DocumentNumber"].ToString();
                supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();
                isPhase1 = Convert.ToInt32(row["isPhase1"] ?? 0) == 1;

                i++;
            }


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
                        cmd.CommandText = "GetSalesInvoiceData";
                        cmd.Parameters.AddWithValue("@IrnNo", irnNo);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@transtype", transType);
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            if (dataReader.Read())
                            {
                                irn = dataReader.MapToObject<IRNMaster>();
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    data = dataReader.MapToObject<CreateOrEditSalesInvoiceDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Items == null)
                                        data.Items = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceItemDto>();
                                    data.Items.Add(items);
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.InvoiceSummary == null)
                                        data.InvoiceSummary = new();
                                    data.InvoiceSummary = dataReader.MapToObject<CreateOrEditSalesInvoiceSummaryDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer == null)
                                        data.Buyer = new();
                                    //if (data.Supplier == null)
                                    //   supplier = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoicePartyDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer.Add(party);
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier.Add(party);
                                    //}
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].Address == null)
                                        data.Buyer[0].Address = new();
                                    //if (data.Supplier[0].Address == null)
                                    //   supplier[0].Address = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceAddressDto>();
                                    if (party.Type == "Buyer" )
                                    {
                                        data.Buyer[0].Address = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].Address = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].ContactPerson == null)
                                        data.Buyer[0].ContactPerson = new();
                                    //if (data.Supplier[0].ContactPerson == null)
                                    //   supplier[0].ContactPerson = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceContactPersonDto>();
                                    if (party.Type == "Buyer" )
                                    {
                                        data.Buyer[0].ContactPerson = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].ContactPerson = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.VATDetails == null)
                                        data.VATDetails = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceVATDetailDto>();
                                    data.VATDetails.Add(items);
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Discount == null)
                                        data.Discount = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceDiscountDto>();
                                    data.Discount.Add(items);
                                }
                            }
                            data.Supplier = new();
                            data.Supplier.Add(supplier);

                            var response = new InvoiceResponse();

                            try
                            {
                                response.PdfFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".pdf";
                                response.XmlFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".xml";
                                response.QRCodeUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".png";

                                if (transType == "388")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Sales
                                    });
                                }

                                if (transType == "383")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Debit
                                    });
                                }

                                if (transType == "381")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Credit
                                    });
                                }
                                //var newURL = Request.Scheme + "://" + Request.Host.Value + "/";

                                var pathToSave = string.Empty;
                                if (tenantId != null && tenantId.ToString() != "")
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, irn.UniqueIdentifier.ToString());
                                else
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", irn.UniqueIdentifier.ToString());
                                if (!Directory.Exists(pathToSave))
                                    Directory.CreateDirectory(pathToSave);
                                var xmlfileName = data.Supplier[0].VATID + "_" + data.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + data.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + irn.IRNNo.ToString() + ".xml";
                                var path = (Path.Combine(pathToSave, xmlfileName));

                                var xmlBase64 = FileIO.GetFileInBas64(path);
                                var pdfBase64 = xmlBase64;
                                var xmlHash = FileIO.GetSha256FileHash(path);
                                var pdfHash = xmlHash;

                                if (transType == "381")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Credit);
                                }
                                if (transType == "383")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Debit);
                                }
                                if (transType == "388")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales);
                                }
                                response.InvoiceNumber = irn.IRNNo.ToString();

                                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, irn.UniqueIdentifier.ToString()));

                                //var emaildata = mapper.Map<InvoiceRequest>(data);
                                //_ = Task.Run(() => EmailSender.SendEmail(pdfResponse.PdfFileUrl, emaildata));


                            }
                            catch (Exception ex)
                            {
                                return new InvoiceResponse();
                            }


                            return new InvoiceResponse()
                            {
                                InvoiceId = Convert.ToInt32(irn.IRNNo.ToString()),
                                InvoiceNumber = data.InvoiceNumber,
                                Uuid = irn.UniqueIdentifier,
                                PdfFileUrl = AzureUpload.GetBlobUrl(pdfResponse.PdfFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                XmlFileUrl = AzureUpload.GetBlobUrl(pdfResponse.XmlFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                QRCodeUrl = AzureUpload.GetBlobUrl(pdfResponse.QRCodeUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/"))
                            };
                        }

                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<InvoiceResponse> GenerateDraftInvoice(int? irnNo, int tenantId, string transType)
        {
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(tenantId);

            InvoiceResponse pdfResponse = new();
            IRNMaster irn = new();
            CreateOrEditSalesInvoiceDto data = new();
            CreateOrEditSalesInvoicePartyDto supplier = new();
            supplier.Address = new();
            supplier.ContactPerson = new();
            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                supplier.RegistrationName = row["TenancyName"].ToString();

                supplier.VATID = row["vatid"].ToString();
                supplier.Website = row["website"].ToString();
                supplier.FaxNo = row["faxNo"].ToString();
                //Address
                supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                supplier.Address.BuildingNo = row["BuildingNo"].ToString();
                supplier.Address.Street = row["Street"].ToString();
                supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                supplier.Address.PostalCode = row["PostalCode"].ToString();
                supplier.Address.CountryCode = row["country"].ToString();
                supplier.Address.City = row["city"].ToString();
                supplier.Address.State = row["State"].ToString();
                supplier.CRNumber = row["DocumentNumber"].ToString();
                supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();
                isPhase1 = Convert.ToInt32(row["isPhase1"] ?? 0) == 1;

                i++;
            }


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
                        cmd.CommandText = "GetDraftInvoiceData";
                        cmd.Parameters.AddWithValue("@IrnNo", irnNo);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@transtype", transType);
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            if (dataReader.Read())
                            {
                                irn = dataReader.MapToObject<IRNMaster>();
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    data = dataReader.MapToObject<CreateOrEditSalesInvoiceDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Items == null)
                                        data.Items = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceItemDto>();
                                    data.Items.Add(items);
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.InvoiceSummary == null)
                                        data.InvoiceSummary = new();
                                    data.InvoiceSummary = dataReader.MapToObject<CreateOrEditSalesInvoiceSummaryDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer == null)
                                        data.Buyer = new();
                                    //if (data.Supplier == null)
                                    //   supplier = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoicePartyDto>();
                                    if (party.Type == "Buyer" && string.IsNullOrEmpty(party.Language))
                                    {
                                        data.Buyer.Add(party);
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier.Add(party);
                                    //}
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].Address == null)
                                        data.Buyer[0].Address = new();
                                    //if (data.Supplier[0].Address == null)
                                    //   supplier[0].Address = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceAddressDto>();
                                    if (party.Type == "Buyer" && string.IsNullOrEmpty(party.Language))
                                    {
                                        data.Buyer[0].Address = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].Address = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].ContactPerson == null)
                                        data.Buyer[0].ContactPerson = new();
                                    //if (data.Supplier[0].ContactPerson == null)
                                    //   supplier[0].ContactPerson = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceContactPersonDto>();
                                    if (party.Type == "Buyer" && string.IsNullOrEmpty(party.Language))
                                    {
                                        data.Buyer[0].ContactPerson = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].ContactPerson = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.VATDetails == null)
                                        data.VATDetails = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceVATDetailDto>();
                                    data.VATDetails.Add(items);
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Discount == null)
                                        data.Discount = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceDiscountDto>();
                                    data.Discount.Add(items);
                                }
                            }
                            data.Supplier = new();
                            data.Supplier.Add(supplier);

                            var response = new InvoiceResponse();

                            try
                            {
                                response.PdfFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".pdf";
                                response.XmlFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".xml";
                                response.QRCodeUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".png";




                                var pathToSave = string.Empty;
                                if (tenantId != null && tenantId.ToString() != "")
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, irn.UniqueIdentifier.ToString());
                                else
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", irn.UniqueIdentifier.ToString());
                                if (!Directory.Exists(pathToSave))
                                    Directory.CreateDirectory(pathToSave);




                                if (transType == "381")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Credit, true);
                                }
                                if (transType == "383")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Debit, true);
                                }
                                if (transType == "388")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales, true);
                                }
                                response.InvoiceNumber = irn.IRNNo.ToString();

                                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, irn.UniqueIdentifier.ToString()));


                            }
                            catch (Exception ex)
                            {
                                return new InvoiceResponse();
                            }


                            return new InvoiceResponse()
                            {
                                InvoiceId = Convert.ToInt32(irn.IRNNo.ToString()),
                                InvoiceNumber = data.InvoiceNumber,
                                Uuid = irn.UniqueIdentifier,
                                PdfFileUrl = AzureUpload.GetBlobUrl(pdfResponse.PdfFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                XmlFileUrl = AzureUpload.GetBlobUrl(pdfResponse.XmlFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                QRCodeUrl = AzureUpload.GetBlobUrl(pdfResponse.QRCodeUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/"))
                            };
                        }

                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}