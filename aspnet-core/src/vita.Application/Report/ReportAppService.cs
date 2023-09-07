using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vita.Report.Dto;
using vita.DataExporting.Excel;
using vita.Dto;
using vita.TenantDetails;
using Abp.Timing.Timezone;
using NPOI.HPSF;
using Abp.Runtime.Session;


namespace vita.Report
{
    public class ReportAppService : vitaAppServiceBase, IReportAppService
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IExcelExporter _excelExporter;
        private readonly ITenantBasicDetailsAppService _tenantbasicdetails;
        private readonly ITimeZoneConverter _timeZoneConverter;



        public ReportAppService(IDbContextProvider<vitaDbContext> dbContextProvider
            , IExcelExporter excelExporter, ITenantBasicDetailsAppService tenantbasicdetails,
            ITimeZoneConverter timeZoneConverter)
        {
            _dbContextProvider = dbContextProvider;
            _excelExporter = excelExporter;
            _tenantbasicdetails = tenantbasicdetails;
            _timeZoneConverter = timeZoneConverter;
        }



        public async Task<DataTable> GetSalesDetailedReport(ReportInputDto input)
        {
            input.Fromdate = _timeZoneConverter.Convert(input.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Fromdate;
            input.Todate = _timeZoneConverter.Convert(input.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Todate;

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
                        cmd.CommandText = "GetSalesDetailedReport";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@fromDate", input.Fromdate);
                        cmd.Parameters.AddWithValue("@toDate", input.Todate);
                        cmd.Parameters.AddWithValue("@code", input.code);


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
            //var invoiceHeaders = _invoiceHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList();
            //List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders)
            //{
            //    SalesReport_VAT dtObj = new SalesReport_VAT();
            //    var items = _invoiceItem.GetAll().Where(a => a.IRNNo == invoice.IRNNo);
            //    dtObj.IRNNo = Convert.ToInt32(invoice.IRNNo);
            //    dtObj.InvoiceNumber = invoice.InvoiceNumber;
            //    dtObj.InvoiceDate = invoice.IssueDate;
            //    dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice);
            //    dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice);
            //    dtObj.HealthCare = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice);
            //    dtObj.Exports = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.VatRate = 15;
            //    dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount);
            //    dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT);
            //    resultSet.Add(dtObj);
            //}


            //return resultSet;

        }

        public async Task<DataTable> GetMasterReport(string code)
        {
            //input.Fromdate = _timeZoneConverter.Convert(input.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Fromdate;
            //input.Todate = _timeZoneConverter.Convert(input.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Todate;

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
                        cmd.CommandText = "masterReport";
                        cmd.Parameters.AddWithValue("@tenantid", AbpSession.TenantId);
                        //cmd.Parameters.AddWithValue("@fromdate", input.Fromdate);
                        //cmd.Parameters.AddWithValue("@todate", input.Todate);
                        cmd.Parameters.AddWithValue("@code", code);


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
            //var invoiceHeaders = _invoiceHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList();
            //List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders)
            //{
            //    SalesReport_VAT dtObj = new SalesReport_VAT();
            //    var items = _invoiceItem.GetAll().Where(a => a.IRNNo == invoice.IRNNo);
            //    dtObj.IRNNo = Convert.ToInt32(invoice.IRNNo);
            //    dtObj.InvoiceNumber = invoice.InvoiceNumber;
            //    dtObj.InvoiceDate = invoice.IssueDate;
            //    dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice);
            //    dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice);
            //    dtObj.HealthCare = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice);
            //    dtObj.Exports = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.VatRate = 15;
            //    dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount);
            //    dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT);
            //    resultSet.Add(dtObj);
            //}


            //return resultSet;

        }

        public async Task<DataTable> GetSalesDayWiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
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
                        cmd.CommandText = "GetSalesDayWiseReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        dt.Load(cmd.ExecuteReader());
                        return dt;
                        conn.Close();

                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
            //var invoiceHeaders = _invoiceHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList();
            //List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders)
            //{
            //    SalesReport_VAT dtObj = new SalesReport_VAT();
            //    var items = _invoiceItem.GetAll().Where(a => a.IRNNo == invoice.IRNNo);
            //    dtObj.IRNNo = Convert.ToInt32(invoice.IRNNo);
            //    dtObj.InvoiceNumber = invoice.InvoiceNumber;
            //    dtObj.InvoiceDate = invoice.IssueDate;
            //    dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice);
            //    dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice);
            //    dtObj.HealthCare = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice);
            //    dtObj.Exports = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.VatRate = 15;
            //    dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount);
            //    dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT);
            //    resultSet.Add(dtObj);
            //}
            //var groupedResultSet = resultSet.GroupBy(p => p.InvoiceDate.Date).ToList();
            //List<SalesReport_VAT> reportResultSet = new List<SalesReport_VAT>();
            //foreach (var group in groupedResultSet)
            //{
            //    SalesReport_VAT dayreportData = new SalesReport_VAT();
            //    dayreportData.InvoiceNumber = group.Count().ToString();
            //    dayreportData.InvoiceDate = group.Key;
            //    dayreportData.TaxableAmount = group.Sum(a => a.TaxableAmount);
            //    dayreportData.ZeroRated = group.Sum(a => a.ZeroRated);
            //    dayreportData.HealthCare = group.Sum(a => a.HealthCare);
            //    dayreportData.Exempt = group.Sum(a => a.Exempt);
            //    dayreportData.Exports = group.Sum(a => a.Exports);
            //    dayreportData.VatAmount = group.Sum(a => a.VatAmount);
            //    dayreportData.TotalAmount = group.Sum(a => a.TotalAmount);
            //    reportResultSet.Add(dayreportData);
            //}


            //return reportResultSet;

        }


        public async Task<DataTable> GetDetailedData(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

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
                        cmd.CommandText = "SP_WHTDetailReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

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

            //List<WHTDetailedReport> result = new List<WHTDetailedReport>();
            //var i = 0;
            //foreach (DataRow row in dt.Rows)
            //{
            //    result.Add(new WHTDetailedReport()
            //    {
            //        SLNO = Convert.ToInt32(row["SLNO"]),
            //        PaymentType = row["TypeofPayment"].ToString(),
            //        PayeeName = row["NameofthePayee"].ToString(),
            //        Country = row["country"].ToString(),
            //        PaymentDate = Convert.ToDateTime(row["paymentdate"]),
            //        TotalAmountPaid = Convert.ToDouble(row["totalamount"]),
            //        TaxRate = Convert.ToDouble(row["taxrate"]),
            //        WithHoldingTaxAmount = Convert.ToDouble(row["WHTamount"])

            //    });
            //    i++;

            //}

            //return result;
        }

        public async Task<DataTable> execSP(string name, DateTime? fromDate, DateTime? toDate)
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
                        cmd.CommandText = name;
                        if (fromDate != null)
                        {
                            cmd.Parameters.AddWithValue("fromDate", fromDate);
                            cmd.Parameters.AddWithValue("toDate", toDate);
                            cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        }
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
        public async Task<DataTable> GetPayemntReturn(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
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
                        cmd.CommandText = "SP_WHTreturnReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

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
            //List<WHTReport> result = new List<WHTReport>();
            //var i = 0;
            //foreach (DataRow row in dt.Rows)
            //{
            //    result.Add(new WHTReport()
            //    {
            //        SLNO = Convert.ToInt32(row["SLNO"]),
            //        PaymentType = row["TypeofPayment"].ToString(),
            //        PayeeName = row["NameofthePayee"].ToString(),
            //        PaymentDate = Convert.ToDateTime(row["paymentdate"]),
            //        TotalAmountPaid = Convert.ToDouble(row["totalamount"]),
            //        TaxRate = Convert.ToDouble(row["taxrate"]),
            //        TaxDue = Convert.ToDouble(row["taxdue"])

            //    });
            //    i++;

            //}

            // return result;
        }


        public async Task<DataTable> GetPurchaseDetailedReport(ReportInputDto Input)
        {
            Input.Fromdate = _timeZoneConverter.Convert(Input.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? Input.Fromdate;
            Input.Todate = _timeZoneConverter.Convert(Input.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? Input.Todate;

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
                        cmd.CommandText = "GetPurchaseDetailedReport";

                        cmd.Parameters.AddWithValue("@fromDate", Input.Fromdate);
                        cmd.Parameters.AddWithValue("@toDate", Input.Todate);
                        cmd.Parameters.AddWithValue("@code", Input.code);
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

            //var filteredTrn_PurchaseDataAggregations = _trn_PurchaseDataAggregationRepository.GetAll().Where(p => p.IssueDate >= fromDate.Date && p.IssueDate <= toDate.Date).ToList();
            //List<Trn_PurchaseDataAggregation> resultSet = new List<Trn_PurchaseDataAggregation>();


            //resultSet = filteredTrn_PurchaseDataAggregations.DistinctBy(p => p.InvoiceNumber).ToList();

            //return resultSet;

        }

        public async Task<DataTable> GetPurchaseDaywiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
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
                        cmd.CommandText = "GetPurchaseDaywiseReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

            //    var filteredTrn_PurchaseDataAggregations = _trn_PurchaseDataAggregationRepository.GetAll().Where(p => p.IssueDate >= fromDate.Date && p.IssueDate <= toDate.Date).ToList();
            //    List<Trn_PurchaseDataAggregation> resultSet = new List<Trn_PurchaseDataAggregation>();
            //    List<Trn_PurchaseDataAggregation> FresultSet = new List<Trn_PurchaseDataAggregation>();


            //    resultSet = filteredTrn_PurchaseDataAggregations.DistinctBy(p => p.InvoiceNumber).ToList();
            //    var groupedResultSet = resultSet.GroupBy(p => p.IssueDate).ToList();
            //    foreach (var group in groupedResultSet)
            //    {
            //        Trn_PurchaseDataAggregation dayreportData = new Trn_PurchaseDataAggregation();
            //        dayreportData.InvoiceNumber = group.Count().ToString();
            //        dayreportData.IssueDate = group.Key;
            //        dayreportData.TotalAmountWithoutVAT = group.Sum(a => a.TotalAmountWithoutVAT);
            //        dayreportData.TotalVATAmount = group.Sum(a => a.TotalVATAmount);
            //        dayreportData.TotalAmountWithVAT = group.Sum(a => a.TotalAmountWithVAT);
            //        FresultSet.Add(dayreportData);
            //    }


            //    return FresultSet;

        }

        public async Task<DataTable> GetDebitNotePeriodicalReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetDebitNotePeriodicalReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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
            //var invoiceHeaders = _debitNoteHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList(); List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders) { SalesReport_VAT dtObj = new SalesReport_VAT(); var items = _itemRepository.GetAll().Where(a => a.IRNno == invoice.IRNno); dtObj.IRNNo = Convert.ToInt32(invoice.IRNno); dtObj.ReferenceNo = invoice.BillingReferenceId; dtObj.InvoiceNumber = invoice.InvoiceNumber; dtObj.InvoiceDate = invoice.IssueDate; dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice); dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice); dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice); dtObj.VatRate = items.Sum(p => p.VATRate); dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount); dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT); resultSet.Add(dtObj); }

            //return resultSet;
        }

        public async Task<DataTable> GetCreditDetailedReport(ReportInputDto Input)
        {
            Input.Fromdate = _timeZoneConverter.Convert(Input.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? Input.Fromdate;
            Input.Todate = _timeZoneConverter.Convert(Input.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? Input.Todate;

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
                        cmd.CommandText = "GetCreditDetailedReport";

                        cmd.Parameters.AddWithValue("@fromDate", Input.Fromdate);
                        cmd.Parameters.AddWithValue("@toDate", Input.Todate);
                        cmd.Parameters.AddWithValue("@code", Input.code);
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

            //var invoiceHeaders = _creditNoteHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList();
            //List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders)
            //{
            //    SalesReport_VAT dtObj = new SalesReport_VAT();
            //    var items = _creditNoteItems.GetAll().Where(a => a.IRNno == invoice.IRNno);
            //    dtObj.IRNNo = Convert.ToInt32(invoice.IRNno);
            //    dtObj.ReferenceNo = invoice.BillingReferenceId;
            //    dtObj.InvoiceNumber = invoice.InvoiceNumber;
            //    dtObj.InvoiceDate = invoice.IssueDate;
            //    dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice);
            //    dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice);
            //    dtObj.HealthCare = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice);
            //    dtObj.Exports = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.VatRate = 15;
            //    dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount);
            //    dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT);
            //    resultSet.Add(dtObj);
            //}


            //return resultSet;

        }

        public async Task<FileDto> GetVatReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();

            try
            {
                var li = await GetAllNew(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);

                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns["text"].ColumnName = "VAT On";
                dt.Columns["VAT"].ColumnName = "VAT Amount";
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileVat(dt, fileName, fromDate, toDate, tenantName);
        }

        public async Task<FileDto> GetWhtDetailedReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            WHTExcelDto input = new WHTExcelDto();


            try
            {
                dt = await GetDetailedData(fromDate, toDate);
                decimal TotalAmount = 0;
                decimal taxrate = 0;
                decimal whtamount = 0;


                foreach (DataRow row in dt.Rows)
                {
                    whtamount += Convert.ToDecimal(row["withholdingtaxamount"]);
                    TotalAmount += Convert.ToDecimal(row["totalamountPaid"]);

                }

                dt.Rows.Add(null, "", "", "", null, TotalAmount, null, whtamount);
                input.FinancialNumber = "";
                input.FiscalYear = "";
                dt.Columns["slno"].ColumnName = "Sl Number";
                dt.Columns["Typeofpayments"].ColumnName = "Type of payments";
                dt.Columns["NameofPayee"].ColumnName = "Name of Payee";
                dt.Columns["country"].ColumnName = "Country";
                dt.Columns["paymentdate"].ColumnName = "Payment Date";
                dt.Columns["totalamountPaid"].ColumnName = "Total Amount Paid";
                dt.Columns["taxrate"].ColumnName = "Tax Rate";
                dt.Columns["withholdingtaxamount"].ColumnName = "With Holding Tax Amount";
                dt.Columns["AffiliationStatus"].ColumnName = "Affiliation Status";


                input.WithholderName = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.ToMonth = toDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileWht(dt, fileName, input);
        }

        public async Task<FileDto> GetWhtReturnReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            WHTExcelDto input = new WHTExcelDto();


            try
            {
                dt = await GetPayemntReturn(fromDate, toDate);
                decimal TotalAmount = 0;
                decimal taxrate = 0;
                decimal whtamount = 0;


                foreach (DataRow row in dt.Rows)
                {
                    whtamount += Convert.ToDecimal(row["totalamountPaid"]);
                    TotalAmount += Convert.ToDecimal(row["taxDue"]);

                }

                dt.Rows.Add(null, "", "", null, whtamount, null, TotalAmount);

                input.FinancialNumber = "";
                input.FiscalYear = "";
                dt.Columns["slno"].ColumnName = "Sl Number";
                dt.Columns["typeofpayments"].ColumnName = "Type of Payment";
                dt.Columns["nameofPayee"].ColumnName = "Name of the Payee(from which tax is withheld)";
                dt.Columns["paymentdate"].ColumnName = "Payment Date";
                dt.Columns["totalamountPaid"].ColumnName = "Total Amount Paid(SAR)";
                dt.Columns["taxrate"].ColumnName = "Tax Rate";
                dt.Columns["taxDue"].ColumnName = "Tax Due";
                input.WithholderName = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.ToMonth = toDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Return";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileWht(dt, fileName, input);
        }

        public async Task<FileDto> GetSalesDetailedToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetSalesDetailedReport(reportInput);
                decimal TaxableAmount = 0;
                decimal GovTaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    GovTaxableAmount += Convert.ToDecimal(row["GovtTaxableAmt"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }


                dt.Columns.Remove("ReferenceNo");
                
                if(reportInput.code == "VATSAL000")
                {
                    
                    dt.Columns.Remove("IRNNo");
                    dt.Rows.Add("",  null,null, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, 0.00, VatAmount, TotalAmount);

                }
                else
                {
                    dt.Rows.Add("", null, null, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, 0.00, VatAmount, TotalAmount);

                }
                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileSales(dt, fileName, input, reportInput.code);
        }


        public async Task<FileDto> GetSalesDaywiseToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetSalesDayWiseReport(fromDate, toDate);
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;
                decimal gov = 0;
                long count = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);
                    count += Convert.ToInt64(row["invoicenumber"]);
                    gov += Convert.ToDecimal(row["GovtTaxableAmt"]);

                }
                dt.Columns["invoicenumber"].ColumnName = "Invoice Count";
                dt.Rows.Add(null, count, TaxableAmount,gov, ZeroRated, Exempt, Export, OutofScope, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileSales(dt, fileName, input,"");
        }

        public async Task<FileDto> GetReportExcel(string Tenantname,string filename,string Type)
        {
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);

            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetMasterReport(Type);  
                input.Name = Tenantname;
                input.Type = Type;
                

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToMaster(dt, filename, input);
        }

        public async Task<FileDto> GetPurchaseToExcel(ReportInputDto repinput, string fileName, string tenantName, bool total = false)
        {
            repinput.Fromdate = _timeZoneConverter.Convert(repinput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Fromdate;
            repinput.Todate = _timeZoneConverter.Convert(repinput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Todate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetPurchaseDetailedReport(repinput);
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;









                //for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        dt.Columns[j].DataType = typeof(string);
                //    }

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    ImportsatCustoms += Convert.ToDecimal(row["ImportsatCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);




                }
                dt.Columns["InvoiceDate"].ColumnName = "Purchase Date";
                dt.Columns["InvoiceNumber"].ColumnName = "Purchase Number";
                if(repinput.code=="VATPUR000")
                {
                    dt.Rows.Add(null, "", TaxableAmount, ZeroRated, ImportsatCustoms, ImportsatRCM, true, true, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, "", 0.00, Exempt, OutofScope, VatAmount, TotalAmount);

                }
                else
                {
                    dt.Rows.Add(null, null,"", TaxableAmount, ZeroRated, ImportsatCustoms, ImportsatRCM, true, true, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, "", 0.00, Exempt, OutofScope, VatAmount, TotalAmount);

                }
                input.Name = tenantName;
                input.FromDate = (repinput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (repinput.Todate).ToString("dd/MM/yyyy");
                input.Month = repinput.Fromdate.ToString("MMM");
                input.Year = repinput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFilePurchase(dt, fileName, input,repinput.code);
        }

        public async Task<FileDto> GetDaywisePurchaseToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            DataTable Tenantdt = new DataTable();

            Tenantdt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in Tenantdt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetPurchaseDaywiseReport(fromDate, toDate);
                int InvoiceNumber = 0;
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;
                foreach (DataRow row in dt.Rows)

                {
                    InvoiceNumber += Convert.ToInt32(row["InvoiceNumber"]);

                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    ImportsatCustoms += Convert.ToDecimal(row["ImportsatCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }
                dt.Columns["InvoiceNumber"].ColumnName = "Purchase Count";
                dt.Columns["InvoiceDate"].ColumnName = "Purchase Date";

                dt.Rows.Add(null, InvoiceNumber, TaxableAmount, ZeroRated, ImportsatCustoms, ImportsatRCM, true, true, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, null, Exempt, OutofScope, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFilePurchase(dt, fileName, input,"");
        }


        public async Task<FileDto> GetCreditDetailedToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetCreditDetailedReport(reportInput);
                decimal TaxableAmount = 0;
                decimal GovTaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    GovTaxableAmount += Convert.ToDecimal(row["GovtTaxableAmt"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }
                dt.Columns["InvoiceDate"].ColumnName = "Credit Note Date";
                dt.Columns["irnno"].ColumnName = "Credit Note Number";
                dt.Columns["Invoicenumber"].ColumnName = "Invoice Number";
                dt.Columns["invoiceNumber1"].ColumnName = "Reference Number";

                if (reportInput.code== "VATCNS000")
                {
                    dt.Rows.Add("", "", "", null,TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, 0.00, VatAmount, TotalAmount);

                }
                else
                {
                    dt.Rows.Add("", "", "", null, null, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, 0.00, VatAmount, TotalAmount);

                }

                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileCredit(dt, fileName, input, reportInput.code);
        }



        public async Task<FileDto> GetCreditDaywiseToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetCreditSalesDaywiseReport(reportInput.Fromdate,reportInput.Todate);
                decimal TaxableAmount = 0;
                decimal GovTaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;
                long count = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    GovTaxableAmount += Convert.ToDecimal(row["GovtTaxableAmt"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);
                    count += Convert.ToInt64(row["invoicenumber"]);


                }
                dt.Columns["invoicedate"].ColumnName = "Credit Note Date";
                dt.Columns["invoicenumber"].ColumnName = "Credit Note count";

                dt.Rows.Add(null, count, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileCredit(dt, fileName, input,"");
        }

        public async Task<List<VatReportDto>> GetAllNew(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_GetVATReport";


                    cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                    cmd.Parameters["FromDate"].Value = fromDate;
                    cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                    cmd.Parameters["ToDate"].Value = toDate;
                    cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                    //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                    cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                    dt.Load(cmd.ExecuteReader());
                    conn.Close();


                }
            }

            List<VatReportDto> result = new List<VatReportDto>();
            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new VatReportDto()
                {
                    Id = i,
                    text = row["vatDescription"].ToString(),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    Adjustment = Convert.ToDecimal(row["AdjustmentAmount"]),
                    Vat = Convert.ToDecimal(row["VatAmount"])
                });
                i++;

            }

            return result;



            //    List<VatReportDto> result = new List<VatReportDto>();
            //result.Add(new VatReportDto()
            //{
            //    Id = 1,//1
            //    text = "1. Standard Rated Sales 15%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 2,//1.1
            //    text = "1.1 Standard Rated Sales 5%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 3,//1.2
            //    text = "1.2 Government supplies sales subject to VAT Standard rate (15%)",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 4,//2
            //    text = "2 Sales on which the government bears the VAT",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 5,//3
            //    text = "3 Zero rated domestic sales",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 6,//4
            //    text = "4 Export",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 7,//5
            //    text = "5 Exempt Sales",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 8,//6
            //    text = "6 Total Sales",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 9,//7
            //    text = "7 Standard Rated Domestic Purchase 15%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 10,//7.1
            //    text = "7.1 Standard Rated Domestic Purchase 5%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 11,//8
            //    text = "8 Imports subject to VAT paid at customs 15%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0

            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 12,//8.1
            //    text = "8.1 Imports subject to VAT paid at customs 5%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0

            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 13,//9 
            //    text = "9 Imports subject to VAT accounted for through reverse charge mechanism 15%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 14,//9.1
            //    text = "9.1 Imports subject to VAT accounted for through reverse charge mechanism 5%",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 15,//10
            //    text = "10 Zero rated purchases",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 16,//11
            //    text = "11 Exempt purchases",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //result.Add(new VatReportDto()
            //{
            //    Id = 16,//12
            //    text = "12 Total purchases",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 17,//13
            //    text = "13 Total VAT due for current period",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 18,//14
            //    text = "14 Corrections from previous period (between SAR 5,000±)",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 19,//15
            //    text = "15 VAT credit carried forward from previous period(s)",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});

            //result.Add(new VatReportDto()
            //{
            //    Id = 20,//16
            //    text = "16 Net VAT due (or claim)",
            //    Amount = 0,
            //    Adjustment = 0,
            //    Vat = 0
            //});
            //return result;
        }

        public async Task<List<VatReportDto>> GetAllNewTax(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

            DataTable dt = new DataTable();
            var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();

                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_GetTaxReturnReport";


                    cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                    cmd.Parameters["FromDate"].Value = fromDate;
                    cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                    cmd.Parameters["ToDate"].Value = toDate;
                    cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                    //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                    cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                    dt.Load(cmd.ExecuteReader());
                    conn.Close();

                }
            }
            List<VatReportDto> result = new List<VatReportDto>();
            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new VatReportDto()
                {
                    Id = i,
                    text = row["vatDescription"].ToString(),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    Adjustment = Convert.ToDecimal(row["AdjustmentAmount"]),
                    Vat = Convert.ToDecimal(row["VatAmount"])
                });
                i++;

            }

            return result;
        }

        public async Task<DataTable> GetCreditSalesDaywiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetCreditNoteDaywiseReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

        public async Task<DataTable> GetCreditPurchaseDaywiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetCreditNotePurchaseDaywiseReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

        public async Task<DataTable> GetCreditPurchaseDetailedReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetCreditNotePurchaseDetailedReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

        public async Task<DataTable> GetDebitPurchaseDetailedReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetDebitNotePurchaseDetailedReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

        public async Task<DataTable> GetDebitPurchaseDayWiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

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
                        cmd.CommandText = "GetDebitNotePurchaseDayWiseReport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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

        public async Task<DataTable> GetDebitSalesDaywiseReport(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
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
                        cmd.CommandText = "getdebitnotedaywisereport";

                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
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



        public async Task<List<VatReportDto>> GetSalesReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetSalesReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                   
                        result.Add(new VatReportDto()
                        {
                            Id = i,
                            text = row["Description"].ToString(),
                            Amount = row["InnerAmount"] == DBNull.Value? null: Convert.ToDecimal(row["InnerAmount"]),
                            Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                            style = Convert.ToInt32(row["style"])
                        });
                        i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }

        public async Task<List<VatReportDto>> GetSalesCreditReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetSalesCreditReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }


        public async Task<List<VatReportDto>> GetSalesDebitReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetSalesDebitReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }

        public async Task<List<VatReportDto>> GetPurchaseReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetPurchaseReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }


        public async Task<List<VatReportDto>> GetPurchaseCreditReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetPurchaseCreditReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }


        public async Task<List<VatReportDto>> GetPurchaseDebitReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetPurchaseDebitReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }


        public async Task<List<VatReportDto>> GetOverheadsReconciliationReport(DateTime fromDate, DateTime toDate)
        {

            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            List<VatReportDto> result = new List<VatReportDto>();

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
                        cmd.CommandText = "GetOverHeadReconciliationReport";


                        cmd.Parameters.Add(new SqlParameter("FromDate", SqlDbType.DateTime));
                        cmd.Parameters["FromDate"].Value = fromDate;
                        cmd.Parameters["FromDate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("ToDate", SqlDbType.DateTime));
                        cmd.Parameters["ToDate"].Value = toDate;
                        cmd.Parameters["ToDate"].Direction = ParameterDirection.Input;
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();


                    }
                }

                var i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new VatReportDto()
                    {
                        Id = i,
                        text = row["Description"].ToString(),
                        Amount = row["InnerAmount"] == DBNull.Value ? null : Convert.ToDecimal(row["InnerAmount"]),
                        Adjustment = row["Amount"] == DBNull.Value ? null : Convert.ToDecimal(row["Amount"]),
                        style = Convert.ToInt32(row["style"])
                    });
                    i++;

                }

                return result;

            }
            catch (Exception e)
            {
                return result;
            }


        }


        public async Task<FileDto> GetSalesReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();

                }
                var li = await GetSalesReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();



                input.Name = tenantName;
                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Sales Invoice Reconciliation Report";
            return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName,headername,input);
        }


        public async Task<FileDto> GetSalesCreditReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();

                }
                var li = await GetSalesCreditReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();



                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Sales Credit Note Reconciliation Report";
           return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }

        public async Task<FileDto> GetSalesDebitReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {

                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();

                }

                var li = await GetSalesDebitReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();

                



                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Sales Debit Note Reconciliation Report";
            return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }


        public async Task<FileDto> GetPurchaseReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {

                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();

                }
                var li = await GetPurchaseReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();



                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Purchase Entry Reconciliation Report";
            return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }

        public async Task<FileDto> GetPurchaseCreditReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();
                }

                    var li = await GetPurchaseCreditReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();


                




                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Purchase Credit Note Reconciliation Report";
           return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }

        public async Task<FileDto> GetPurchaseDebitReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();
                }
                    var li = await GetPurchaseDebitReconciliationReport(fromDate, toDate);
                
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();



                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;




            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Purchase Debit Note Reconciliation Report";
           return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }

        public async Task<FileDto> GetOverheadsReconciliationReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            try
            {
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.VAT = row["vatid"].ToString();
                    input.Address = row["State"].ToString() + "," + row["country"].ToString();

                }
                var li = await GetOverheadsReconciliationReport(fromDate, toDate);
                dt = _excelExporter.ToDataTable<VatReportDto>(li, total);
                List<VatReportDto> result = new List<VatReportDto>();





                dt.Columns.Remove("id");
                dt.Columns.Remove("style");
                dt.Columns.Remove("Vat");
                dt.Columns["text"].ColumnName = "Description";
                dt.Columns["Amount"].ColumnName = " ";
                dt.Columns["Adjustment"].ColumnName = "Amount";
                input.Name = tenantName;




            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            string headername = "Overheads Reconciliation Report";
            return _excelExporter.ExportToFileSalesReconciliation(dt, fileName, fromDate, toDate, tenantName, headername,input);
        }

        public async Task<FileDto> GetDebitDetailedToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetDebitNotePeriodicalReport(reportInput.Fromdate,reportInput.Todate);
                decimal TaxableAmount = 0;
                decimal GovTaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    GovTaxableAmount += Convert.ToDecimal(row["GovtTaxableAmt"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }
               // dt.Columns.Remove("ReferenceNo");
                dt.Columns["invoicenumber"].ColumnName = "Invoice Number";
                dt.Columns["invoicedate"].ColumnName = "Debit Note Date";
                dt.Columns["IRNNo"].ColumnName = "Debit Note Number";

                dt.Rows.Add("", "", null,null, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, 0.00, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileDebit(dt, fileName, input);
        }


        public async Task<FileDto> GetDebitDaywiseToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetDebitSalesDaywiseReport(reportInput.Fromdate, reportInput.Todate);
                decimal TaxableAmount = 0;
                decimal GovTaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal Export = 0;
                decimal OutofScope = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;
                long count = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    GovTaxableAmount += Convert.ToDecimal(row["GovtTaxableAmt"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    Export += Convert.ToDecimal(row["Exports"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);
                    count += Convert.ToInt64(row["invoicenumber"]);

                }
                dt.Columns["invoicedate"].ColumnName = "Debit Note Date";
                dt.Columns["invoicenumber"].ColumnName = "Debit Note Count";
                dt.Rows.Add(null, count, TaxableAmount, GovTaxableAmount, ZeroRated, Export, Exempt, OutofScope, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileDebit(dt, fileName, input);
        }


        public async Task<FileDto> GetCreditPurchaseToExcel(ReportInputDto repinput, string fileName, string tenantName, bool total = false)
        {
            repinput.Fromdate = _timeZoneConverter.Convert(repinput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Fromdate;
            repinput.Todate = _timeZoneConverter.Convert(repinput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Todate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetCreditPurchaseDetailedReport(repinput.Fromdate,repinput.Todate);
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;









                //for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        dt.Columns[j].DataType = typeof(string);
                //    }

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    ImportsatCustoms += Convert.ToDecimal(row["importVATCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);




                }
                dt.Columns["invoicenumber"].ColumnName = "Credit Note Number";
                dt.Columns["invoiceDate"].ColumnName = "Credit Note Date";

                dt.Rows.Add(null, "","",null,"", TaxableAmount, ZeroRated, Exempt, OutofScope, 0, true, 0, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, 0.00, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (repinput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (repinput.Todate).ToString("dd/MM/yyyy");
                input.Month = repinput.Fromdate.ToString("MMM");
                input.Year = repinput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileCreditPurchase(dt, fileName, input);
        }


        public async Task<FileDto> GetDaywiseCreditPurchaseToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            DataTable Tenantdt = new DataTable();

            Tenantdt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in Tenantdt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetCreditPurchaseDaywiseReport(fromDate, toDate);
                int InvoiceNumber = 0;
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;









                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                //    dt.Columns[j].DataType = typeof(string);
                //}

                foreach (DataRow row in dt.Rows)

                {
                    InvoiceNumber += Convert.ToInt32(row["invoicecount"]);
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    ImportsatCustoms += Convert.ToDecimal(row["ImportVATCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }
                dt.Columns["invoicecount"].ColumnName = "Credit Note Count";
                dt.Columns["invoiceDate"].ColumnName = "Credit Note Date";
                dt.Rows.Add(null, InvoiceNumber, null, TaxableAmount, ZeroRated, Exempt, OutofScope, 0, true, 0, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileCreditPurchase(dt, fileName, input);
        }


        public async Task<FileDto> GetDebitPurchaseToExcel(ReportInputDto repinput, string fileName, string tenantName, bool total = false)
        {
            repinput.Fromdate = _timeZoneConverter.Convert(repinput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Fromdate;
            repinput.Todate = _timeZoneConverter.Convert(repinput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? repinput.Todate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();

            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetDebitPurchaseDetailedReport(repinput.Fromdate, repinput.Todate);
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;









                //for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        dt.Columns[j].DataType = typeof(string);
                //    }

                foreach (DataRow row in dt.Rows)
                {
                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    ImportsatCustoms += Convert.ToDecimal(row["importVATCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);




                }
                dt.Columns["invoicenumber"].ColumnName = "Debit Note Number";
                dt.Columns["invoiceDate"].ColumnName = "Debit Note Date";
                dt.Rows.Add(null, "", "", null, "", TaxableAmount, ZeroRated, Exempt, OutofScope, 0, true, 0, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, 0.00, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (repinput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (repinput.Todate).ToString("dd/MM/yyyy");
                input.Month = repinput.Fromdate.ToString("MMM");
                input.Year = repinput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileDebitPurchase(dt, fileName, input);
        }


        public async Task<FileDto> GetDaywiseDebitPurchaseToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, bool total = false)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            DataTable Tenantdt = new DataTable();

            Tenantdt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in Tenantdt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetDebitPurchaseDayWiseReport(fromDate, toDate);
                int InvoiceNumber = 0;
                decimal TaxableAmount = 0;
                decimal ZeroRated = 0;
                decimal Exempt = 0;
                decimal OutofScope = 0;
                decimal ImportsatCustoms = 0;
                decimal ImportsatRCM = 0;
                decimal CustomsPaid = 0;
                decimal ExciseTaxPaid = 0;
                decimal OtherChargesPaid = 0;
                decimal VatAmount = 0;
                decimal TotalAmount = 0;









                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                //    dt.Columns[j].DataType = typeof(string);
                //}

                foreach (DataRow row in dt.Rows)

                {
                    InvoiceNumber += Convert.ToInt32(row["InvoiceNumber"]);

                    TaxableAmount += Convert.ToDecimal(row["TaxableAmount"]);
                    ZeroRated += Convert.ToDecimal(row["ZeroRated"]);
                    Exempt += Convert.ToDecimal(row["Exempt"]);
                    ImportsatCustoms += Convert.ToDecimal(row["importVATCustoms"]);
                    ImportsatRCM += Convert.ToDecimal(row["ImportsatRCM"]);
                    CustomsPaid += Convert.ToDecimal(row["CustomsPaid"]);
                    ExciseTaxPaid += Convert.ToDecimal(row["ExciseTaxPaid"]);
                    OtherChargesPaid += Convert.ToDecimal(row["OtherChargesPaid"]);
                    OutofScope += Convert.ToDecimal(row["OutofScope"]);
                    VatAmount += Convert.ToDecimal(row["VatAmount"]);
                    TotalAmount += Convert.ToDecimal(row["TotalAmount"]);

                }
                dt.Columns["InvoiceNumber"].ColumnName = "Debit Note Count";
                dt.Columns["invoiceDate"].ColumnName = "Debit Note Date";
                dt.Rows.Add(null, InvoiceNumber, null, TaxableAmount, ZeroRated, Exempt, OutofScope, 0, true, 0, CustomsPaid, ExciseTaxPaid, OtherChargesPaid, VatAmount, TotalAmount);
                input.Name = tenantName;
                input.FromDate = (fromDate).ToString("dd/MM/yyyy");
                input.ToDate = (toDate).ToString("dd/MM/yyyy");
                input.Month = fromDate.ToString("MMM");
                input.Year = fromDate.Year.ToString();
                input.Type = "Daywise";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileDebitPurchase(dt, fileName, input);
        }

        public async Task<DataTable> GetOverrideReport(ReportInputDto input)
        {
            input.Fromdate = _timeZoneConverter.Convert(input.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Fromdate;
            input.Todate = _timeZoneConverter.Convert(input.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.Todate;

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
                        cmd.CommandText = "GetOverrideReport";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@fromDate", input.Fromdate);
                        cmd.Parameters.AddWithValue("@toDate", input.Todate);
                        cmd.Parameters.AddWithValue("@code", input.code);


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
            //var invoiceHeaders = _invoiceHeaderRepository.GetAll().Where(p => p.IssueDate.Date >= fromDate.Date && p.IssueDate.Date <= toDate.Date).ToList();
            //List<SalesReport_VAT> resultSet = new List<SalesReport_VAT>();

            //foreach (var invoice in invoiceHeaders)
            //{
            //    SalesReport_VAT dtObj = new SalesReport_VAT();
            //    var items = _invoiceItem.GetAll().Where(a => a.IRNNo == invoice.IRNNo);
            //    dtObj.IRNNo = Convert.ToInt32(invoice.IRNNo);
            //    dtObj.InvoiceNumber = invoice.InvoiceNumber;
            //    dtObj.InvoiceDate = invoice.IssueDate;
            //    dtObj.TaxableAmount = items.Where(a => a.VATCode == "S").Sum(p => p.NetPrice);
            //    dtObj.ZeroRated = items.Where(a => a.VATCode == "Z").Sum(p => p.NetPrice);
            //    dtObj.HealthCare = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.Exempt = items.Where(a => a.VATCode == "E").Sum(p => p.NetPrice);
            //    dtObj.Exports = items.Where(a => a.VATCode == "O").Sum(p => p.NetPrice);
            //    dtObj.VatRate = 15;
            //    dtObj.VatAmount = items.Where(a => a.VATCode == "S").Sum(p => p.VATAmount);
            //    dtObj.TotalAmount = items.Sum(p => p.LineAmountInclusiveVAT);
            //    resultSet.Add(dtObj);
            //}


            //return resultSet;

        }


        public async Task<FileDto> GetOverrideReportToExcel(ReportInputDto reportInput, string fileName, string tenantName, bool total = false)
        {
            reportInput.Fromdate = _timeZoneConverter.Convert(reportInput.Fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Fromdate;
            reportInput.Todate = _timeZoneConverter.Convert(reportInput.Todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? reportInput.Todate;

            DataTable dt = new DataTable();
            PurchaseExcelDto input = new PurchaseExcelDto();
            //ReportInputDto repinput = new ReportInputDto();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.VAT = row["vatid"].ToString();
                input.Address = row["State"].ToString() + "," + row["country"].ToString();

            }

            try
            {
                dt = await GetOverrideReport(reportInput);
                input.Name = tenantName;
                input.Month = reportInput.Fromdate.ToString("MMM");
                input.FromDate = (reportInput.Fromdate).ToString("dd/MM/yyyy");
                input.ToDate = (reportInput.Todate).ToString("dd/MM/yyyy");
                input.Year = reportInput.Fromdate.Year.ToString();
                input.Type = "Detailed";


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return _excelExporter.ExportToFileOverride(dt, fileName, input);
        }
        public async Task<DataTable> GetReportType(string inputCode)
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
                        cmd.CommandText = "GetReportType";                      
                        cmd.Parameters.AddWithValue("@code", inputCode);


                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }                  
                }
            }
            catch (Exception e)
            {
                return dt;
            }


        }

    }
    
}
