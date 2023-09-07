using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Payment;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using Abp.Timing.Timezone;

namespace vita.PaymentFileUpload
{
    public class ImportPaymentFileUploadAppService : vitaAppServiceBase ,IPaymentFileUpload
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;


        public ImportPaymentFileUploadAppService(IDbContextProvider<vitaDbContext> dbContextProvider, ITimeZoneConverter timeZoneConverter)
        {
            _dbContextProvider = dbContextProvider;
            _timeZoneConverter = timeZoneConverter;

        }

        public async Task<bool> InsertBatchUploadPayment(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate)
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
                        cmd.CommandText = "InsertBatchUploadPayment";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);



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
