using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Vendor.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using System.Data;
using Microsoft.Data.SqlClient;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using Abp.Timing.Timezone;
using Newtonsoft.Json;
using vita.Customer.Dtos;

namespace vita.Vendor
{
    [AbpAuthorize(AppPermissions.Pages_Vendorses)]
    public class VendorsesAppService : vitaAppServiceBase, IVendorsesAppService
    {
        private readonly IRepository<Vendors, long> _vendorsRepository;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;



        public VendorsesAppService(IRepository<Vendors, long> vendorsRepository, IDbContextProvider<vitaDbContext> dbContextProvider, ITimeZoneConverter timeZoneConverter)
        {
            _vendorsRepository = vendorsRepository;
            _dbContextProvider =  dbContextProvider;
            _timeZoneConverter = timeZoneConverter;


        }

        public async Task<bool> InsertBatchUploadVendor(string json, string fileName, int? tenantId)
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
                        cmd.CommandText = "InsertBatchUploadVendor";

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


        public async Task<bool> InsertBatchUploadLedger(string json, string fileName, int? tenantId)
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
                        cmd.CommandText = "InsertBatchUploadLedger";

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

        public async Task<DataTable> GetVendorForEdit(int id)
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
                        cmd.CommandText = "GetVendorForEdit";


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



        public async Task<DataTable> GetVendorData()
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
                        cmd.CommandText = "GetVendorData";
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

        public async Task<DataTable> GetVendorName(String Name)
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
                        cmd.CommandText = "GetVendorName";

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

        public async Task<DataTable> GetVendorById(int Id)
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
                        cmd.CommandText = "GetVendorById";


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



        public async Task<bool> CreateVendor(CreateOrEditVendorsDto input)
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
                        cmd.CommandText = "InsertVendorData";

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

        public async Task<bool> upadateVendor(CreateOrEditVendorsDto input)
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
                        cmd.CommandText = "UpadateVendorData";

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

        public async Task<PagedResultDto<GetVendorsForViewDto>> GetAll(GetAllVendorsesInput input)
        {

            var filteredVendorses = _vendorsRepository.GetAll()
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

            var pagedAndFilteredVendorses = filteredVendorses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorses = from o in pagedAndFilteredVendorses
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

            var totalCount = await filteredVendorses.CountAsync();

            var dbList = await vendorses.ToListAsync();
            var results = new List<GetVendorsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorsForViewDto()
                {
                    Vendors = new VendorsDto
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

            return new PagedResultDto<GetVendorsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Vendorses_Edit)]
        public async Task<GetVendorsForEditOutput> GetVendorsForEdit(EntityDto<long> input)
        {
            var vendors = await _vendorsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorsForEditOutput { Vendors = ObjectMapper.Map<CreateOrEditVendorsDto>(vendors) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Vendorses_Create)]
        protected virtual async Task Create(CreateOrEditVendorsDto input)
        {
            var vendors = ObjectMapper.Map<Vendors>(input);

            if (AbpSession.TenantId != null)
            {
                vendors.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorsRepository.InsertAsync(vendors);

        }

        [AbpAuthorize(AppPermissions.Pages_Vendorses_Edit)]
        protected virtual async Task Update(CreateOrEditVendorsDto input)
        {
            var vendors = await _vendorsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendors);

        }

        [AbpAuthorize(AppPermissions.Pages_Vendorses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorsRepository.DeleteAsync(input.Id);
        }

    }
}