using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.TenantDetails.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using vita.Customer.Dtos;
using Abp.Timing.Timezone;

namespace vita.TenantDetails
{
    public class TenantBasicDetailsAppService : vitaAppServiceBase, ITenantBasicDetailsAppService
    {
        private readonly IRepository<TenantBasicDetails> _tenantBasicDetailsRepository;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;



        public TenantBasicDetailsAppService(IRepository<TenantBasicDetails> tenantBasicDetailsRepository,
             IDbContextProvider<vitaDbContext> dbContextProvider,
             ITimeZoneConverter timeZoneConverter)
        {
            _tenantBasicDetailsRepository = tenantBasicDetailsRepository;
            _dbContextProvider = dbContextProvider;
            _timeZoneConverter = timeZoneConverter;


        }

        public async Task<DataTable> GetTenantById(int? Id)
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
                        cmd.CommandText = "GetTenantById";


                        cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.Int));
                        cmd.Parameters["Id"].Value = Id;
                        cmd.Parameters["Id"].Direction = ParameterDirection.Input;

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



        public async Task<DataTable> GetTenantpartnerinfoById(int? Id)
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
                        cmd.CommandText = "GetTenantpartnerinfoById";


                        cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.Int));
                        cmd.Parameters["Id"].Value = Id;
                        cmd.Parameters["Id"].Direction = ParameterDirection.Input;

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


        public async Task<bool> CreateTenant(CreateOrEditTenantBasicDetailsDto input)
        {

            var json = JsonConvert.SerializeObject(input);
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
                        cmd.CommandText = "InsertTenantDetails";

                        cmd.Parameters.AddWithValue("json", json);



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


        public async Task<bool> InsertBatchUploadTenant(string json, string fileName, int? tenantId)
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
                        cmd.CommandText = "InsertUpdateTenantDetails";

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
        public async Task<bool> upadateTenant(CreateOrEditTenantBasicDetailsDto input)
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
                        cmd.CommandText = "UpadateTenantData";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@id", AbpSession.TenantId);



                        int i = cmd.ExecuteNonQuery();

                        return true;
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


        public async Task<PagedResultDto<GetTenantBasicDetailsForViewDto>> GetAll(GetAllTenantBasicDetailsInput input)
        {

            var filteredTenantBasicDetails = _tenantBasicDetailsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TenantType.Contains(input.Filter) || e.ConstitutionType.Contains(input.Filter) || e.BusinessCategory.Contains(input.Filter) || e.OperationalModel.Contains(input.Filter) || e.TurnoverSlab.Contains(input.Filter) || e.ContactPerson.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.EmailID.Contains(input.Filter) || e.Nationality.Contains(input.Filter) || e.Designation.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.ParentEntityName.Contains(input.Filter) || e.LegalRepresentative.Contains(input.Filter) || e.ParentEntityCountryCode.Contains(input.Filter) || e.LastReturnFiled.Contains(input.Filter) || e.VATReturnFillingFrequency.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantTypeFilter), e => e.TenantType.Contains(input.TenantTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConstitutionTypeFilter), e => e.ConstitutionType.Contains(input.ConstitutionTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessCategoryFilter), e => e.BusinessCategory.Contains(input.BusinessCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OperationalModelFilter), e => e.OperationalModel.Contains(input.OperationalModelFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TurnoverSlabFilter), e => e.TurnoverSlab.Contains(input.TurnoverSlabFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactPersonFilter), e => e.ContactPerson.Contains(input.ContactPersonFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailIDFilter), e => e.EmailID.Contains(input.EmailIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NationalityFilter), e => e.Nationality.Contains(input.NationalityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DesignationFilter), e => e.Designation.Contains(input.DesignationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentEntityNameFilter), e => e.ParentEntityName.Contains(input.ParentEntityNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalRepresentativeFilter), e => e.LegalRepresentative.Contains(input.LegalRepresentativeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentEntityCountryCodeFilter), e => e.ParentEntityCountryCode.Contains(input.ParentEntityCountryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastReturnFiledFilter), e => e.LastReturnFiled.Contains(input.LastReturnFiledFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATReturnFillingFrequencyFilter), e => e.VATReturnFillingFrequency.Contains(input.VATReturnFillingFrequencyFilter));

            var pagedAndFilteredTenantBasicDetails = filteredTenantBasicDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantBasicDetails = from o in pagedAndFilteredTenantBasicDetails
                                     select new
                                     {

                                         o.TenantType,
                                         o.ConstitutionType,
                                         o.BusinessCategory,
                                         o.OperationalModel,
                                         o.TurnoverSlab,
                                         o.ContactPerson,
                                         o.ContactNumber,
                                         o.EmailID,
                                         o.Nationality,
                                         o.Designation,
                                         o.VATID,
                                         o.ParentEntityName,
                                         o.LegalRepresentative,
                                         o.ParentEntityCountryCode,
                                         o.LastReturnFiled,
                                         o.VATReturnFillingFrequency,
                                         Id = o.Id
                                     };

            var totalCount = await filteredTenantBasicDetails.CountAsync();

            var dbList = await tenantBasicDetails.ToListAsync();
            var results = new List<GetTenantBasicDetailsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantBasicDetailsForViewDto()
                {
                    TenantBasicDetails = new TenantBasicDetailsDto
                    {

                        TenantType = o.TenantType,
                        ConstitutionType = o.ConstitutionType,
                        BusinessCategory = o.BusinessCategory,
                        OperationalModel = o.OperationalModel,
                        TurnoverSlab = o.TurnoverSlab,
                        ContactPerson = o.ContactPerson,
                        ContactNumber = o.ContactNumber,
                        EmailID = o.EmailID,
                        Nationality = o.Nationality,
                        Designation = o.Designation,
                        VATID = o.VATID,
                        ParentEntityName = o.ParentEntityName,
                        LegalRepresentative = o.LegalRepresentative,
                        ParentEntityCountryCode = o.ParentEntityCountryCode,
                        LastReturnFiled = o.LastReturnFiled,
                        VATReturnFillingFrequency = o.VATReturnFillingFrequency,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantBasicDetailsForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTenantBasicDetailsForViewDto> GetTenantBasicDetailsForView(int id)
        {
            var tenantBasicDetails = await _tenantBasicDetailsRepository.GetAsync(id);

            var output = new GetTenantBasicDetailsForViewDto { TenantBasicDetails = ObjectMapper.Map<TenantBasicDetailsDto>(tenantBasicDetails) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantBasicDetails_Edit)]
        public async Task<GetTenantBasicDetailsForEditOutput> GetTenantBasicDetailsForEdit(EntityDto input)
        {
            var tenantBasicDetails = await _tenantBasicDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantBasicDetailsForEditOutput { TenantBasicDetails = ObjectMapper.Map<CreateOrEditTenantBasicDetailsDto>(tenantBasicDetails) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantBasicDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantBasicDetails_Create)]
        protected virtual async Task Create(CreateOrEditTenantBasicDetailsDto input)
        {
            var tenantBasicDetails = ObjectMapper.Map<TenantBasicDetails>(input);

            if (AbpSession.TenantId != null)
            {
                tenantBasicDetails.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantBasicDetailsRepository.InsertAsync(tenantBasicDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBasicDetails_Edit)]
        protected virtual async Task Update(CreateOrEditTenantBasicDetailsDto input)
        {
            var tenantBasicDetails = await _tenantBasicDetailsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantBasicDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBasicDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantBasicDetailsRepository.DeleteAsync(input.Id);
        }


        public async Task<DataTable> GetTenantBatchData(string fileName)
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
                        cmd.CommandText = "GetTenantBatchData";
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

        public async Task<DataTable> getbusinesssuppliesdropdown()
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
                        cmd.CommandText = "getbusinesssuppliesdropdown";



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

        public async Task<DataTable> getbusinessPurchasedropdown()
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
                        cmd.CommandText = "getbusinessPurchasedropdown";



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


        public async Task<DataTable> getsalesvatdropdown()
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
                        cmd.CommandText = "getsalesvatdropdown";



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

        public async Task<DataTable> getpurchasevatdropdown()
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
                        cmd.CommandText = "getpurchasevatdropdown";



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


    }



}