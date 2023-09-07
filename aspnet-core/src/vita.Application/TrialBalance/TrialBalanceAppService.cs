using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.EntityFrameworkCore;
using vita.Vendor;

namespace vita.TrialBalance
{
    public class TrialBalanceAppService : vitaAppServiceBase, ITrialBalancesesAppService
    {
        private readonly IRepository<Vendors, long> _vendorsRepository;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;


        public TrialBalanceAppService(IRepository<Vendors, long> vendorsRepository, IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _vendorsRepository = vendorsRepository;
            _dbContextProvider = dbContextProvider;

        }
        public async Task<bool> InsertBatchUploadTrailBalance(string json, string fileName, int? tenantId)
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
                        cmd.CommandText = "InsertBatchUploadTrailBalance";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);



                        int i = cmd.ExecuteNonQuery();
                        conn.Close();

                        return true;

                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
