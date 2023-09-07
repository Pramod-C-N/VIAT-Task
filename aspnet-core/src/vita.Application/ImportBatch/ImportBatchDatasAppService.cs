using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.ImportBatch.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using vita.EntityFrameworkCore;
using System.Text.Json;

using System.Data;
using vita.ImportBatch.Exporting;
using vita.Sales;
using vita.Customer;
using Abp.Timing.Timezone;

namespace vita.ImportBatch
{
    [AbpAuthorize(AppPermissions.Pages_ImportBatchDatas)]
    public class ImportBatchDatasAppService : vitaAppServiceBase, IImportBatchDatasAppService
    {
        private readonly IRepository<ImportBatchData, long> _importBatchDataRepository;
        private readonly ISalesFileExcelExporter _salesFIleExcelExporter;
        private readonly ISalesInvoicesAppService _salesInvoicesAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ICustomersesAppService _customersesAppService;
        private readonly ITimeZoneConverter _timeZoneConverter;


        public ImportBatchDatasAppService(IRepository<ImportBatchData, long> importBatchDataRepository, ISalesInvoicesAppService salesInvoicesAppService, ISalesFileExcelExporter salesdFIleExcelExporter,
            IDbContextProvider<vitaDbContext> dbContextProvider, ICustomersesAppService customersesAppService, ITimeZoneConverter timeZoneConverter)
        {
            _importBatchDataRepository = importBatchDataRepository;
            _salesFIleExcelExporter = salesdFIleExcelExporter;
            _salesInvoicesAppService = salesInvoicesAppService;
            _dbContextProvider = dbContextProvider;
            _customersesAppService = customersesAppService;
            _timeZoneConverter = timeZoneConverter;


        }

        public async Task<bool> OverrideErrors(List<OverRideDto> uniqueId, int batchId)
        {
            try
            {
                var json = JsonSerializer.Serialize(uniqueId);
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "OverrideErrors";



                        cmd.Parameters.AddWithValue("@batchId", batchId);
                        cmd.Parameters.AddWithValue("@json", json);


                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        await cmd.ExecuteNonQueryAsync();
                        conn.Close();


                    }


                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<bool> OverrideErrorsMasters(List<OverRideDto> uniqueId, int batchId)
        {
            try
            {
                var json = JsonSerializer.Serialize(uniqueId);
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "OverRideErrorsMasters";



                        cmd.Parameters.AddWithValue("@batchId", batchId);
                        cmd.Parameters.AddWithValue("@json", json);


                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        cmd.ExecuteNonQuery();
                        conn.Close();


                    }


                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<DataTable> execgetdataSP(int batchid,int? para=null)
        {

            DataTable dt = new DataTable();
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
                        cmd.CommandText = "GetReportDataByID";


                        cmd.Parameters.Add(new SqlParameter("batchid", SqlDbType.Int));
                        cmd.Parameters["batchid"].Value = batchid;
                        cmd.Parameters["batchid"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(new SqlParameter("para", SqlDbType.Int));
                        cmd.Parameters["para"].Value = para;
                        cmd.Parameters["para"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }


                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }   
        public async Task<DataTable> GetMasterReportDataByID(int batchid,int? para=null)
        {

            DataTable dt = new DataTable();
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
                        cmd.CommandText = "GetMasterReportDataByID";


                        cmd.Parameters.Add(new SqlParameter("batchid", SqlDbType.Int));
                        cmd.Parameters["batchid"].Value = batchid;
                        cmd.Parameters["batchid"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(new SqlParameter("para", SqlDbType.Int));
                        cmd.Parameters["para"].Value = para;
                        cmd.Parameters["para"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }


                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }
        public async Task<DataTable> getReportData(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetBatchData";
                        cmd.Parameters.AddWithValue("fromDate", fromDate);
                        cmd.Parameters.AddWithValue("toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetMasterReportData(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetMasterBatchData";
                        cmd.Parameters.AddWithValue("fromDate", fromDate);
                        cmd.Parameters.AddWithValue("toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }


        public async Task<List<ImportBatchDataDto>> GetReportDataByFile(string fileName)
        {

            var results = new List<ImportBatchDataDto>();




            return results;

        }

        //private FileDto ExportToFile(DataTable dt, string fileName)
        //{
        //    return CreateExcelPackage(
        //            fileName,
        //            excelPackage =>
        //            {

        //                var sheet = excelPackage.CreateSheet("SalesDetail");
        //                string[] row = new string[dt.Columns.Count];
        //                for (var i = 0; i < dt.Columns.Count; i++)
        //                    row[i] = dt.Columns[i].ColumnName;

        //                AddHeader(sheet, row);

        //                AddObjectsFromDatatable(sheet, dt);
        //            });

        //}

        public async Task<string> GetMasterTypeFromBatchId(int batchId)
        {
            string masterType = "";
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
                        cmd.CommandText = "GetMasterTypeFromBatchId";

                        cmd.Parameters.AddWithValue("@batchId", batchId);

                        var reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                           masterType = reader.GetString(0);
                        }
                        conn.Close();


                    }


                    return masterType;
                }
            }
            catch (Exception e)
            {
                return masterType;
            }
        }

        public async Task<FileDto> GetInvalidRecordsToExcel(int Batchid, string fileName)
        {

            //TODO - replace with method to get invalid data
            var dt = await _salesInvoicesAppService.GetSalesInvalidRecord(Batchid);
            return _salesFIleExcelExporter.ExportToFile(dt, "Invalid_" + fileName);
        }

   
        public async Task<FileDto> GetMasterInvalidRecordsToExcel(int Batchid, string fileName)
        {

            //TODO - replace with method to get invalid data
            var dt = await _customersesAppService.GetMasterInvalidRecord(Batchid);
            string masterType = await GetMasterTypeFromBatchId(Batchid);
            if(masterType == "CustomerData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt, "Invalid_" + fileName, "CUSTOMER MASTER FILE UPLOAD - INVALID RECORDS FOR CORRECTION");
            else if (masterType == "TenantData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt, "Invalid_" + fileName, "TENANT MASTER FILE UPLOAD - INVALID RECORDS FOR CORRECTION");
            else if (masterType == "VendorData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt, "Invalid_" + fileName, "VENDOR MASTER FILE UPLOAD - INVALID RECORDS FOR CORRECTION");
            else
            return _salesFIleExcelExporter.ExportToFile(dt, "Invalid_" + fileName);
        }
        public async Task<FileDto> GetErrorListToExcel(string fileName, int batchid,int? para)
        {

            //TODO - replace with method to get invalid data
            //var dt = await _salesInvoicesAppService.GetSalesBatchData(fileName);
            var dt = await execgetdataSP(batchid,para);

            return _salesFIleExcelExporter.ExportToFile(dt, fileName);
        }

        public async Task<FileDto> GetMasterErrorListToExcel(string fileName, int batchid,int para)
        {

            //TODO - replace with method to get invalid data
            //var dt = await _salesInvoicesAppService.GetSalesBatchData(fileName);
            var dt = await GetMasterReportDataByID(batchid,para);
            string masterType = await GetMasterTypeFromBatchId(batchid);
            if (masterType == "CustomerData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt, fileName, "CUSTOMER MASTER FILE UPLOAD - VALID & INVALID RECORDS FOR CORRECTION");
            else if(masterType == "TenantData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt,  fileName, "TENANT MASTER FILE UPLOAD - VALID & INVALID  RECORDS FOR CORRECTION");
            else if (masterType == "VendorData")
                return _salesFIleExcelExporter.ExportToFileWithCustomHeader(dt,  fileName, "VENDOR MASTER FILE UPLOAD - VALID & INVALID  RECORDS FOR CORRECTION");
            else
                return _salesFIleExcelExporter.ExportToFile(dt,  fileName);
        }

        public async Task<PagedResultDto<GetImportBatchDataForViewDto>> GetAll(GetAllImportBatchDatasInput input)
        {

            var filteredImportBatchDatas = _importBatchDataRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Filename.Contains(input.Filter) || e.InvoiceType.Contains(input.Filter) || e.IRNNo.Contains(input.Filter) || e.InvoiceNumber.Contains(input.Filter) || e.IssueTime.Contains(input.Filter) || e.InvoiceCurrencyCode.Contains(input.Filter) || e.PurchaseOrderId.Contains(input.Filter) || e.ContractId.Contains(input.Filter) || e.BuyerMasterCode.Contains(input.Filter) || e.BuyerName.Contains(input.Filter) || e.BuyerVatCode.Contains(input.Filter) || e.BuyerContact.Contains(input.Filter) || e.BuyerCountryCode.Contains(input.Filter) || e.InvoiceLineIdentifier.Contains(input.Filter) || e.ItemMasterCode.Contains(input.Filter) || e.ItemName.Contains(input.Filter) || e.UOM.Contains(input.Filter) || e.VatCategoryCode.Contains(input.Filter) || e.VatExemptionReasonCode.Contains(input.Filter) || e.VatExemptionReason.Contains(input.Filter) || e.Error.Contains(input.Filter) || e.BillingReferenceId.Contains(input.Filter) || e.ReasonForCN.Contains(input.Filter) || e.BillOfEntry.Contains(input.Filter) || e.PurchaseNumber.Contains(input.Filter) || e.PurchaseCategory.Contains(input.Filter) || e.LedgerHeader.Contains(input.Filter) || e.TransType.Contains(input.Filter) || e.AdvanceRcptRefNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter) || e.NatureofServices.Contains(input.Filter) || e.PlaceofSupply.Contains(input.Filter) || e.OrgType.Contains(input.Filter) || e.AffiliationStatus.Contains(input.Filter) || e.PerCapitaHoldingForiegnCo.Contains(input.Filter) || e.CapitalInvestedbyForeignCompany.Contains(input.Filter) || e.CapitalInvestmentCurrency.Contains(input.Filter) || 
e.VendorConstitution.Contains(input.Filter))
                        .WhereIf(input.MinBatchIdFilter != null, e => e.BatchId >= input.MinBatchIdFilter)
                        .WhereIf(input.MaxBatchIdFilter != null, e => e.BatchId <= input.MaxBatchIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FilenameFilter), e => e.Filename.Contains(input.FilenameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceTypeFilter), e => e.InvoiceType.Contains(input.InvoiceTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNumberFilter), e => e.InvoiceNumber.Contains(input.InvoiceNumberFilter))
                        .WhereIf(input.MinIssueDateFilter != null, e => e.IssueDate >= input.MinIssueDateFilter)
                        .WhereIf(input.MaxIssueDateFilter != null, e => e.IssueDate <= input.MaxIssueDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IssueTimeFilter), e => e.IssueTime.Contains(input.IssueTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceCurrencyCodeFilter), e => e.InvoiceCurrencyCode.Contains(input.InvoiceCurrencyCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseOrderIdFilter), e => e.PurchaseOrderId.Contains(input.PurchaseOrderIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContractIdFilter), e => e.ContractId.Contains(input.ContractIdFilter))
                        .WhereIf(input.MinSupplyDateFilter != null, e => e.SupplyDate >= input.MinSupplyDateFilter)
                        .WhereIf(input.MaxSupplyDateFilter != null, e => e.SupplyDate <= input.MaxSupplyDateFilter)
                        .WhereIf(input.MinSupplyEndDateFilter != null, e => e.SupplyEndDate >= input.MinSupplyEndDateFilter)
                        .WhereIf(input.MaxSupplyEndDateFilter != null, e => e.SupplyEndDate <= input.MaxSupplyEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerMasterCodeFilter), e => e.BuyerMasterCode.Contains(input.BuyerMasterCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerNameFilter), e => e.BuyerName.Contains(input.BuyerNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerVatCodeFilter), e => e.BuyerVatCode.Contains(input.BuyerVatCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerContactFilter), e => e.BuyerContact.Contains(input.BuyerContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerCountryCodeFilter), e => e.BuyerCountryCode.Contains(input.BuyerCountryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceLineIdentifierFilter), e => e.InvoiceLineIdentifier.Contains(input.InvoiceLineIdentifierFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemMasterCodeFilter), e => e.ItemMasterCode.Contains(input.ItemMasterCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemNameFilter), e => e.ItemName.Contains(input.ItemNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM.Contains(input.UOMFilter))
                        .WhereIf(input.MinGrossPriceFilter != null, e => e.GrossPrice >= input.MinGrossPriceFilter)
                        .WhereIf(input.MaxGrossPriceFilter != null, e => e.GrossPrice <= input.MaxGrossPriceFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.Discount >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.Discount <= input.MaxDiscountFilter)
                        .WhereIf(input.MinNetPriceFilter != null, e => e.NetPrice >= input.MinNetPriceFilter)
                        .WhereIf(input.MaxNetPriceFilter != null, e => e.NetPrice <= input.MaxNetPriceFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinLineNetAmountFilter != null, e => e.LineNetAmount >= input.MinLineNetAmountFilter)
                        .WhereIf(input.MaxLineNetAmountFilter != null, e => e.LineNetAmount <= input.MaxLineNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VatCategoryCodeFilter), e => e.VatCategoryCode.Contains(input.VatCategoryCodeFilter))
                        .WhereIf(input.MinVatRateFilter != null, e => e.VatRate >= input.MinVatRateFilter)
                        .WhereIf(input.MaxVatRateFilter != null, e => e.VatRate <= input.MaxVatRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VatExemptionReasonCodeFilter), e => e.VatExemptionReasonCode.Contains(input.VatExemptionReasonCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VatExemptionReasonFilter), e => e.VatExemptionReason.Contains(input.VatExemptionReasonFilter))
                        .WhereIf(input.MinVATLineAmountFilter != null, e => e.VATLineAmount >= input.MinVATLineAmountFilter)
                        .WhereIf(input.MaxVATLineAmountFilter != null, e => e.VATLineAmount <= input.MaxVATLineAmountFilter)
                        .WhereIf(input.MinLineAmountInclusiveVATFilter != null, e => e.LineAmountInclusiveVAT >= input.MinLineAmountInclusiveVATFilter)
                        .WhereIf(input.MaxLineAmountInclusiveVATFilter != null, e => e.LineAmountInclusiveVAT <= input.MaxLineAmountInclusiveVATFilter)
                        .WhereIf(input.MinProcessedFilter != null, e => e.Processed >= input.MinProcessedFilter)
                        .WhereIf(input.MaxProcessedFilter != null, e => e.Processed <= input.MaxProcessedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorFilter), e => e.Error.Contains(input.ErrorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingReferenceIdFilter), e => e.BillingReferenceId.Contains(input.BillingReferenceIdFilter))
                        .WhereIf(input.MinOrignalSupplyDateFilter != null, e => e.OrignalSupplyDate >= input.MinOrignalSupplyDateFilter)
                        .WhereIf(input.MaxOrignalSupplyDateFilter != null, e => e.OrignalSupplyDate <= input.MaxOrignalSupplyDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReasonForCNFilter), e => e.ReasonForCN.Contains(input.ReasonForCNFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillOfEntryFilter), e => e.BillOfEntry.Contains(input.BillOfEntryFilter))
                        .WhereIf(input.MinBillOfEntryDateFilter != null, e => e.BillOfEntryDate >= input.MinBillOfEntryDateFilter)
                        .WhereIf(input.MaxBillOfEntryDateFilter != null, e => e.BillOfEntryDate <= input.MaxBillOfEntryDateFilter)
                        .WhereIf(input.MinCustomsPaidFilter != null, e => e.CustomsPaid >= input.MinCustomsPaidFilter)
                        .WhereIf(input.MaxCustomsPaidFilter != null, e => e.CustomsPaid <= input.MaxCustomsPaidFilter)
                        .WhereIf(input.MinCustomTaxFilter != null, e => e.CustomTax >= input.MinCustomTaxFilter)
                        .WhereIf(input.MaxCustomTaxFilter != null, e => e.CustomTax <= input.MaxCustomTaxFilter)
                        .WhereIf(input.WHTApplicableFilter.HasValue && input.WHTApplicableFilter > -1, e => (input.WHTApplicableFilter == 1 && e.WHTApplicable) || (input.WHTApplicableFilter == 0 && !e.WHTApplicable))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseNumberFilter), e => e.PurchaseNumber.Contains(input.PurchaseNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseCategoryFilter), e => e.PurchaseCategory.Contains(input.PurchaseCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LedgerHeaderFilter), e => e.LedgerHeader.Contains(input.LedgerHeaderFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransTypeFilter), e => e.TransType.Contains(input.TransTypeFilter))
                        .WhereIf(input.MinAdvanceRcptAmtAdjustedFilter != null, e => e.AdvanceRcptAmtAdjusted >= input.MinAdvanceRcptAmtAdjustedFilter)
                        .WhereIf(input.MaxAdvanceRcptAmtAdjustedFilter != null, e => e.AdvanceRcptAmtAdjusted <= input.MaxAdvanceRcptAmtAdjustedFilter)
                        .WhereIf(input.MinVatOnAdvanceRcptAmtAdjustedFilter != null, e => e.VatOnAdvanceRcptAmtAdjusted >= input.MinVatOnAdvanceRcptAmtAdjustedFilter)
                        .WhereIf(input.MaxVatOnAdvanceRcptAmtAdjustedFilter != null, e => e.VatOnAdvanceRcptAmtAdjusted <= input.MaxVatOnAdvanceRcptAmtAdjustedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdvanceRcptRefNoFilter), e => e.AdvanceRcptRefNo.Contains(input.AdvanceRcptRefNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NatureofServicesFilter), e => e.NatureofServices.Contains(input.NatureofServicesFilter))
                        .WhereIf(input.IsapportionmentFilter.HasValue && input.IsapportionmentFilter > -1, e => (input.IsapportionmentFilter == 1 && e.Isapportionment) || (input.IsapportionmentFilter == 0 && !e.Isapportionment))
                        .WhereIf(input.MinExciseTaxPaidFilter != null, e => e.ExciseTaxPaid >= input.MinExciseTaxPaidFilter)
                        .WhereIf(input.MaxExciseTaxPaidFilter != null, e => e.ExciseTaxPaid <= input.MaxExciseTaxPaidFilter)
                        .WhereIf(input.MinOtherChargesPaidFilter != null, e => e.OtherChargesPaid >= input.MinOtherChargesPaidFilter)
                        .WhereIf(input.MaxOtherChargesPaidFilter != null, e => e.OtherChargesPaid <= input.MaxOtherChargesPaidFilter)
                        .WhereIf(input.MinTotalTaxableAmountFilter != null, e => e.TotalTaxableAmount >= input.MinTotalTaxableAmountFilter)
                        .WhereIf(input.MaxTotalTaxableAmountFilter != null, e => e.TotalTaxableAmount <= input.MaxTotalTaxableAmountFilter)
                        .WhereIf(input.VATDefferedFilter.HasValue && input.VATDefferedFilter > -1, e => (input.VATDefferedFilter == 1 && e.VATDeffered) || (input.VATDefferedFilter == 0 && !e.VATDeffered))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PlaceofSupplyFilter), e => e.PlaceofSupply.Contains(input.PlaceofSupplyFilter))
                        .WhereIf(input.RCMApplicableFilter.HasValue && input.RCMApplicableFilter > -1, e => (input.RCMApplicableFilter == 1 && e.RCMApplicable) || (input.RCMApplicableFilter == 0 && !e.RCMApplicable))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrgTypeFilter), e => e.OrgType.Contains(input.OrgTypeFilter))
                        .WhereIf(input.MinExchangeRateFilter != null, e => e.ExchangeRate >= input.MinExchangeRateFilter)
                        .WhereIf(input.MaxExchangeRateFilter != null, e => e.ExchangeRate <= input.MaxExchangeRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AffiliationStatusFilter), e => e.AffiliationStatus.Contains(input.AffiliationStatusFilter))
                        .WhereIf(input.MinReferenceInvoiceAmountFilter != null, e => e.ReferenceInvoiceAmount >= input.MinReferenceInvoiceAmountFilter)
                        .WhereIf(input.MaxReferenceInvoiceAmountFilter != null, e => e.ReferenceInvoiceAmount <= input.MaxReferenceInvoiceAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PerCapitaHoldingForiegnCoFilter), e => e.PerCapitaHoldingForiegnCo.Contains(input.PerCapitaHoldingForiegnCoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CapitalInvestedbyForeignCompanyFilter), e => e.CapitalInvestedbyForeignCompany.Contains(input.CapitalInvestedbyForeignCompanyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CapitalInvestmentCurrencyFilter), e => e.CapitalInvestmentCurrency.Contains(input.CapitalInvestmentCurrencyFilter))
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.CapitalInvestmentDateFilter), e => e.CapitalInvestmentDate.Contains(input.CapitalInvestmentDateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorConstitutionFilter), e => e.VendorConstitution.Contains(input.VendorConstitutionFilter));

            var pagedAndFilteredImportBatchDatas = filteredImportBatchDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var importBatchDatas = from o in pagedAndFilteredImportBatchDatas
                                   select new
                                   {

                                       o.BatchId,
                                       o.Filename,
                                       o.InvoiceType,
                                       o.IRNNo,
                                       o.InvoiceNumber,
                                       o.IssueDate,
                                       o.IssueTime,
                                       o.InvoiceCurrencyCode,
                                       o.PurchaseOrderId,
                                       o.ContractId,
                                       o.SupplyDate,
                                       o.SupplyEndDate,
                                       o.BuyerMasterCode,
                                       o.BuyerName,
                                       o.BuyerVatCode,
                                       o.BuyerContact,
                                       o.BuyerCountryCode,
                                       o.InvoiceLineIdentifier,
                                       o.ItemMasterCode,
                                       o.ItemName,
                                       o.UOM,
                                       o.GrossPrice,
                                       o.Discount,
                                       o.NetPrice,
                                       o.Quantity,
                                       o.LineNetAmount,
                                       o.VatCategoryCode,
                                       o.VatRate,
                                       o.VatExemptionReasonCode,
                                       o.VatExemptionReason,
                                       o.VATLineAmount,
                                       o.LineAmountInclusiveVAT,
                                       o.Processed,
                                       o.Error,
                                       o.BillingReferenceId,
                                       o.OrignalSupplyDate,
                                       o.ReasonForCN,
                                       o.BillOfEntry,
                                       o.BillOfEntryDate,
                                       o.CustomsPaid,
                                       o.CustomTax,
                                       o.WHTApplicable,
                                       o.PurchaseNumber,
                                       o.PurchaseCategory,
                                       o.LedgerHeader,
                                       o.TransType,
                                       o.AdvanceRcptAmtAdjusted,
                                       o.VatOnAdvanceRcptAmtAdjusted,
                                       o.AdvanceRcptRefNo,
                                       o.PaymentMeans,
                                       o.PaymentTerms,
                                       o.NatureofServices,
                                       o.Isapportionment,
                                       o.ExciseTaxPaid,
                                       o.OtherChargesPaid,
                                       o.TotalTaxableAmount,
                                       o.VATDeffered,
                                       o.PlaceofSupply,
                                       o.RCMApplicable,
                                       o.OrgType,
                                       o.ExchangeRate,
                                       o.AffiliationStatus,
                                       o.ReferenceInvoiceAmount,
                                       o.PerCapitaHoldingForiegnCo,
                                       o.CapitalInvestedbyForeignCompany,
                                       o.CapitalInvestmentCurrency,
                                       o.CapitalInvestmentDate,
                                       o.VendorConstitution,
                                       Id = o.Id
                                   };

            var totalCount = await filteredImportBatchDatas.CountAsync();

            var dbList = await importBatchDatas.ToListAsync();
            var results = new List<GetImportBatchDataForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetImportBatchDataForViewDto()
                {
                    ImportBatchData = new ImportBatchDataDto
                    {

                        BatchId = o.BatchId,
                        Filename = o.Filename,
                        InvoiceType = o.InvoiceType,
                        IRNNo = o.IRNNo,
                        InvoiceNumber = o.InvoiceNumber,
                        IssueDate = o.IssueDate,
                        IssueTime = o.IssueTime,
                        InvoiceCurrencyCode = o.InvoiceCurrencyCode,
                        PurchaseOrderId = o.PurchaseOrderId,
                        ContractId = o.ContractId,
                        SupplyDate = o.SupplyDate,
                        SupplyEndDate = o.SupplyEndDate,
                        BuyerMasterCode = o.BuyerMasterCode,
                        BuyerName = o.BuyerName,
                        BuyerVatCode = o.BuyerVatCode,
                        BuyerContact = o.BuyerContact,
                        BuyerCountryCode = o.BuyerCountryCode,
                        InvoiceLineIdentifier = o.InvoiceLineIdentifier,
                        ItemMasterCode = o.ItemMasterCode,
                        ItemName = o.ItemName,
                        UOM = o.UOM,
                        GrossPrice = o.GrossPrice,
                        Discount = o.Discount,
                        NetPrice = o.NetPrice,
                        Quantity = o.Quantity,
                        LineNetAmount = o.LineNetAmount,
                        VatCategoryCode = o.VatCategoryCode,
                        VatRate = o.VatRate,
                        VatExemptionReasonCode = o.VatExemptionReasonCode,
                        VatExemptionReason = o.VatExemptionReason,
                        VATLineAmount = o.VATLineAmount,
                        LineAmountInclusiveVAT = o.LineAmountInclusiveVAT,
                        Processed = o.Processed,
                        Error = o.Error,
                        BillingReferenceId = o.BillingReferenceId,
                        OrignalSupplyDate = o.OrignalSupplyDate,
                        ReasonForCN = o.ReasonForCN,
                        BillOfEntry = o.BillOfEntry,
                        BillOfEntryDate = o.BillOfEntryDate,
                        CustomsPaid = o.CustomsPaid,
                        CustomTax = o.CustomTax,
                        WHTApplicable = o.WHTApplicable,
                        PurchaseNumber = o.PurchaseNumber,
                        PurchaseCategory = o.PurchaseCategory,
                        LedgerHeader = o.LedgerHeader,
                        TransType = o.TransType,
                        AdvanceRcptAmtAdjusted = o.AdvanceRcptAmtAdjusted,
                        VatOnAdvanceRcptAmtAdjusted = o.VatOnAdvanceRcptAmtAdjusted,
                        AdvanceRcptRefNo = o.AdvanceRcptRefNo,
                        PaymentMeans = o.PaymentMeans,
                        PaymentTerms = o.PaymentTerms,
                        NatureofServices = o.NatureofServices,
                        Isapportionment = o.Isapportionment,
                        ExciseTaxPaid = o.ExciseTaxPaid,
                        OtherChargesPaid = o.OtherChargesPaid,
                        TotalTaxableAmount = o.TotalTaxableAmount,
                        VATDeffered = o.VATDeffered,
                        PlaceofSupply = o.PlaceofSupply,
                        RCMApplicable = o.RCMApplicable,
                        OrgType = o.OrgType,
                        ExchangeRate = o.ExchangeRate,
                        AffiliationStatus = o.AffiliationStatus,
                        ReferenceInvoiceAmount = o.ReferenceInvoiceAmount,
                        PerCapitaHoldingForiegnCo = o.PerCapitaHoldingForiegnCo,
                        CapitalInvestedbyForeignCompany = o.CapitalInvestedbyForeignCompany,
                        CapitalInvestmentCurrency = o.CapitalInvestmentCurrency,
                        CapitalInvestmentDate = o.CapitalInvestmentDate,
                        VendorConstitution = o.VendorConstitution,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetImportBatchDataForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_ImportBatchDatas_Edit)]
        public async Task<GetImportBatchDataForEditOutput> GetImportBatchDataForEdit(EntityDto<long> input)
        {
            var importBatchData = await _importBatchDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetImportBatchDataForEditOutput { ImportBatchData = ObjectMapper.Map<CreateOrEditImportBatchDataDto>(importBatchData) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditImportBatchDataDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ImportBatchDatas_Create)]
        protected virtual async Task Create(CreateOrEditImportBatchDataDto input)
        {
            var importBatchData = ObjectMapper.Map<ImportBatchData>(input);

            if (AbpSession.TenantId != null)
            {
                importBatchData.TenantId = (int?)AbpSession.TenantId;
            }

            await _importBatchDataRepository.InsertAsync(importBatchData);

        }

        [AbpAuthorize(AppPermissions.Pages_ImportBatchDatas_Edit)]
        protected virtual async Task Update(CreateOrEditImportBatchDataDto input)
        {
            var importBatchData = await _importBatchDataRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, importBatchData);

        }

        [AbpAuthorize(AppPermissions.Pages_ImportBatchDatas_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _importBatchDataRepository.DeleteAsync(input.Id);
        }


        public async Task<DataTable> getBatchNumber(string irrno)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getInvoiceSuggestions";
                        cmd.Parameters.AddWithValue("@irrno", irrno);
                        cmd.Parameters.AddWithValue("@refNo", null);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<int> DeleteBatchSummary(int batchId, int mastTrans)
        {
            int masterType = 0;
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
                        cmd.CommandText = "BatchDelete";

                        cmd.Parameters.AddWithValue("@batchno", batchId);
                        cmd.Parameters.AddWithValue("@MastTrans", mastTrans);
                        cmd.Parameters.AddWithValue("@tenantid", AbpSession.TenantId);

                        masterType = cmd.ExecuteNonQuery();

                        conn.Close();


                    }


                    return masterType;
                }
            }
            catch (Exception e)
            {
                return masterType;
            }
        }


    }
}