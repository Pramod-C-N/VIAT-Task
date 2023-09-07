using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.EntityFrameworkCore;
using vita.Organizations;
using vita.OverheadApportionment.Dto;

namespace vita.OverheadApportionment
{
    public class OverheadApportionmentAppService : vitaAppServiceBase, IOverheadApportionment
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        public OverheadApportionmentAppService(
            IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<bool> CreateOverheadApportionmentCurrentDataDetailed(List<OverheadApportionmentCurrentDataDetailedDTO> input)
        {
            try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                for (var i = 0; i < input.Count; i++)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            conn.Open();

                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "insertoverheadPrevdata";
                            cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                            cmd.Parameters.AddWithValue("@TaxableSupplies", input[i].TaxableSupplies);
                            cmd.Parameters.AddWithValue("@ExemptSupplies", input[i].ExemptSupplies);
                            cmd.Parameters.AddWithValue("@ExemptTaxableSupplies", input[i].ExemptTaxableSupplies);
                            cmd.Parameters.AddWithValue("@TaxablePurchase", input[i].TaxablePurchase);
                            cmd.Parameters.AddWithValue("@ExemptPurchase", input[i].ExemptPurchase);
                            cmd.Parameters.AddWithValue("@ExemptTaxablePurchase", input[i].ExemptTaxablePurchase);
                            cmd.Parameters.AddWithValue("@PercentageofTaxable", input[i].PercentageofTaxable);
                            cmd.Parameters.AddWithValue("@Type", input[i].Type);
                            cmd.Parameters.AddWithValue("@Date", input[i].Date);
                            cmd.ExecuteNonQuery();
                            conn.Close();

                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> CreateOverheadApportionmentCurrentDataSummary(OverheadApportionmentPreviousDataDTO input)
        {
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
                        cmd.CommandText = "insertoverheadPrevdata";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@TaxableSupplies", input.TaxableSupplies);
                        cmd.Parameters.AddWithValue("@ExemptSupplies", input.ExemptSupplies);
                        cmd.Parameters.AddWithValue("@ExemptTaxableSupplies", input.ExemptTaxableSupplies);
                        cmd.Parameters.AddWithValue("@TaxablePurchase", input.TaxablePurchase);
                        cmd.Parameters.AddWithValue("@ExemptPurchase", input.ExemptPurchase);
                        cmd.Parameters.AddWithValue("@ExemptTaxablePurchase", input.ExemptTaxablePurchase);
                        cmd.Parameters.AddWithValue("@PercentageofTaxable", input.PercentageofTaxable);
                        cmd.Parameters.AddWithValue("@Type", input.Type);
                        cmd.Parameters.AddWithValue("@Date", input.Date);

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

        public async Task<bool> CreateOverheadApportionmentPreviousData(OverheadApportionmentPreviousDataDTO input)
        {
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
                        cmd.CommandText = "insertoverheadPrevdata";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@TaxableSupplies", input.TaxableSupplies);
                        cmd.Parameters.AddWithValue("@ExemptSupplies", input.ExemptSupplies);
                        cmd.Parameters.AddWithValue("@ExemptTaxableSupplies", input.ExemptTaxableSupplies);
                        cmd.Parameters.AddWithValue("@TaxablePurchase", input.TaxablePurchase);
                        cmd.Parameters.AddWithValue("@ExemptPurchase", input.ExemptPurchase);
                        cmd.Parameters.AddWithValue("@ExemptTaxablePurchase", input.ExemptTaxablePurchase);
                        cmd.Parameters.AddWithValue("@PercentageofTaxable", input.PercentageofTaxable);
                        cmd.Parameters.AddWithValue("@Type", input.Type);
                        cmd.Parameters.AddWithValue("@Date", input.Date);

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

        public async Task<DataTable> GetOverheadApportionmentCurrentDataDetailed(string type)
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
                        cmd.CommandText = "getOverheadApportionmentPreviousData";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@type", type);
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

        public async Task<DataTable> GetOverheadApportionmentCurrentDataSummary()
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
                        cmd.CommandText = "getOverheadApportionmentPreviousData";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@type", "Summary");
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

        public async Task<DataTable> GetOverheadApportionmentPreviousData()
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
                        cmd.CommandText = "getOverheadApportionmentPreviousData";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@type", "Previous");
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
