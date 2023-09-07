using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using System;
using vita.EInvoicing.Dto;
using vita.EntityFrameworkCore;
using vita.Filters;
using JsonFlatten;
using UblSharp;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Abp.Timing.Timezone;
using static vita.Filters.VitaFilter_Authorization;

namespace vita.Filters
{
    public class AuthorizationFilter { 
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITimeZoneConverter _timeZoneConverter;

        public IAbpSession AbpSession { get; set; }

        public AuthorizationFilter(IDbContextProvider<vitaDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager, IAbpSession abpSession, ITimeZoneConverter timeZoneConverter)
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
            AbpSession = abpSession;
            _timeZoneConverter = timeZoneConverter;

        }
        public async Task<bool> CheckIsAuthorized(VitaFilter_ModuleName vitaFilter_ModuleName)
        {
            //bool isAuthorized = false;
            //SqlConnection conn = null;
            //try
            //{
            //    using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            //    {

            //        var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
            //        using (conn = new SqlConnection(connStr))
            //        {

            //            using (SqlCommand cmd = new SqlCommand())
            //            {

            //                conn.Open();
            //                cmd.Connection = conn;
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.CommandText = "CheckIsAuthorized";


            //                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
            //                cmd.Parameters.AddWithValue("@moduleId", vitaFilter_ModuleName);


            //                var reader = cmd.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                    if (!reader.IsDBNull(0))
            //                        isAuthorized = reader.GetInt32(0)==1;
            //                    break;
            //                }
            //                conn.Close();
            //                return isAuthorized;
            //            }
            //        }
            //        await unitOfWork.CompleteAsync();
            //    }

            //    return isAuthorized;
            //}
            //catch (Exception e)
            //{

            //    return isAuthorized;

            //}
            //finally
            //{
            //    if (conn != null && conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }

            //}
            return true;
        }


    }

}
