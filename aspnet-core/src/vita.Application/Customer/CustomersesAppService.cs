using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Customer.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using Newtonsoft.Json;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using NPOI.HPSF;
using System.Data;
using vita.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Abp.Timing.Timezone;

namespace vita.Customer
{
    [AbpAuthorize(AppPermissions.Pages_Customerses)]
    public class CustomersesAppService : vitaAppServiceBase, ICustomersesAppService
    {
        private readonly IRepository<Customers, long> _customersRepository;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;


        public CustomersesAppService(IRepository<Customers, long> customersRepository,
            IDbContextProvider<vitaDbContext> dbContextProvider,
            ITimeZoneConverter timeZoneConverter)
        {
            _customersRepository = customersRepository;
            _dbContextProvider= dbContextProvider;
            _timeZoneConverter = timeZoneConverter;


        }

        public async Task<bool> InsertBatchUploadCustomer(string json, string fileName, int? tenantId)
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
                        cmd.CommandText = "InsertBatchUploadCustomer";

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

        public async Task<DataTable> GetCustomerBatchData(string fileName)
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
                        cmd.CommandText = "GetCustomerBatchData";
                        cmd.Parameters.AddWithValue("@fileName", fileName);
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

        public async Task<DataTable> GetMasterInvalidRecord(int batchid)
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
                        cmd.CommandText = "GetMasterInvalidRecord";
                        cmd.Parameters.AddWithValue("@batchid", batchid);
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


        public async Task<PagedResultDto<GetCustomersForViewDto>> GetAll(GetAllCustomersesInput input)
        {

            var filteredCustomerses = _customersRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TenantType.Contains(input.Filter) || e.ConstitutionType.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.LegalName.Contains(input.Filter) || e.ContactPerson.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.EmailID.Contains(input.Filter) || e.Nationality.Contains(input.Filter) || e.Designation.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantTypeFilter), e => e.TenantType.Contains(input.TenantTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConstitutionTypeFilter), e => e.ConstitutionType.Contains(input.ConstitutionTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalNameFilter), e => e.LegalName.Contains(input.LegalNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactPersonFilter), e => e.ContactPerson.Contains(input.ContactPersonFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailIDFilter), e => e.EmailID.Contains(input.EmailIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NationalityFilter), e => e.Nationality.Contains(input.NationalityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DesignationFilter), e => e.Designation.Contains(input.DesignationFilter));

            var pagedAndFilteredCustomerses = filteredCustomerses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerses = from o in pagedAndFilteredCustomerses
                              select new
                              {

                                  o.TenantType,
                                  o.ConstitutionType,
                                  o.Name,
                                  o.LegalName,
                                  o.ContactPerson,
                                  o.ContactNumber,
                                  o.EmailID,
                                  o.Nationality,
                                  o.Designation,
                                  Id = o.Id
                              };

            var totalCount = await filteredCustomerses.CountAsync();

            var dbList = await customerses.ToListAsync();
            var results = new List<GetCustomersForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomersForViewDto()
                {
                    Customers = new CustomersDto
                    {

                        TenantType = o.TenantType,
                        ConstitutionType = o.ConstitutionType,
                        Name = o.Name,
                        LegalName = o.LegalName,
                        ContactPerson = o.ContactPerson,
                        ContactNumber = o.ContactNumber,
                        EmailID = o.EmailID,
                        Nationality = o.Nationality,
                        Designation = o.Designation,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomersForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<DataTable> GetCustomerForEdit(int id)
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
                        cmd.CommandText = "GetCustomerForEdit";


                        cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int));
                        cmd.Parameters["id"].Value = id;
                        cmd.Parameters["id"].Direction = ParameterDirection.Input;
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



        public async Task<DataTable> GetCustomerData()
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
                        cmd.CommandText = "GetCustomerData";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);




                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                }

                

                return dt;
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetCustomerName(String Name)
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
                        cmd.CommandText = "GetCustomerName";

                        cmd.Parameters.Add(new SqlParameter("Name", SqlDbType.Text));
                        cmd.Parameters["Name"].Value = Name;
                        cmd.Parameters["Name"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                }


                return dt;
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetCustomerById(int Id)
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
                        cmd.CommandText = "GetCustomerById";


                        cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.Int));
                        cmd.Parameters["Id"].Value = Id;
                        cmd.Parameters["Id"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0))

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                return dt;
            }
        }



        [AbpAuthorize(AppPermissions.Pages_Customerses_Edit)]
        public async Task<GetCustomersForEditOutput> GetCustomersForEdit(EntityDto<long> input)
        {
            var customers = await _customersRepository.FirstOrDefaultAsync(input.Id);
            
            var output = new GetCustomersForEditOutput { Customers = ObjectMapper.Map<CreateOrEditCustomersDto>(customers) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomersDto input)
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

        public async Task<bool> CreateCustomer(CreateOrEditCustomersDto input)
        {
            var json = JsonConvert.SerializeObject(input);
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
                        cmd.CommandText = "InsertCustomerData";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);



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

        public async Task<bool> upadateCustomer(CreateOrEditCustomersDto input)
        {
            
            var json = JsonConvert.SerializeObject(input);
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
                        cmd.CommandText = "UpadateCustomerData";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int));
                        cmd.Parameters["id"].Value = input.Id;
                        cmd.Parameters["id"].Direction = ParameterDirection.Input;


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




        [AbpAuthorize(AppPermissions.Pages_Customerses_Create)]
        protected virtual async Task Create(CreateOrEditCustomersDto input)
        {
            var customers = ObjectMapper.Map<Customers>(input);

            if (AbpSession.TenantId != null)
            {
                customers.TenantId = (int?)AbpSession.TenantId;
            }

            await _customersRepository.InsertAsync(customers);

        }

        [AbpAuthorize(AppPermissions.Pages_Customerses_Edit)]
        protected virtual async Task Update(CreateOrEditCustomersDto input)
        {
            var customers = await _customersRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customers);

        }

        [AbpAuthorize(AppPermissions.Pages_Customerses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customersRepository.DeleteAsync(input.Id);
        }

    }
}