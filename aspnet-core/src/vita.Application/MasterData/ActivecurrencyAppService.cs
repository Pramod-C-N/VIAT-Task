using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Exporting;
using vita.MasterData.Dtos;
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
using System.Data;
using vita.Report.Dto;
using vita.EntityFrameworkCore;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_Activecurrency)]
    public class ActivecurrencyAppService : vitaAppServiceBase, IActivecurrencyAppService
    {
        private readonly IRepository<Activecurrency> _activecurrencyRepository;
        private readonly IActivecurrencyExcelExporter _activecurrencyExcelExporter;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;

        public ActivecurrencyAppService(IRepository<Activecurrency> activecurrencyRepository, IActivecurrencyExcelExporter activecurrencyExcelExporter, IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _activecurrencyRepository = activecurrencyRepository;
            _activecurrencyExcelExporter = activecurrencyExcelExporter;
            _dbContextProvider = dbContextProvider;

        }

        public async Task<DataTable> GetActiveCurrencies(string alpha3code)

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
                        cmd.CommandText = "GetActiveCurrencies";
                        cmd.Parameters.AddWithValue("@alpha3code", alpha3code);
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
        public async Task<PagedResultDto<GetActivecurrencyForViewDto>> GetAll(GetAllActivecurrencyInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredActivecurrency = _activecurrencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Entity.Contains(input.Filter) || e.Currency.Contains(input.Filter) || e.AlphabeticCode.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.MinorUnit.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EntityFilter), e => e.Entity.Contains(input.EntityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyFilter), e => e.Currency.Contains(input.CurrencyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AlphabeticCodeFilter), e => e.AlphabeticCode.Contains(input.AlphabeticCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinorUnitFilter), e => e.MinorUnit.Contains(input.MinorUnitFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredActivecurrency = filteredActivecurrency
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var activecurrency = from o in pagedAndFilteredActivecurrency
                                     select new
                                     {

                                         o.Entity,
                                         o.Currency,
                                         o.AlphabeticCode,
                                         o.NumericCode,
                                         o.MinorUnit,
                                         o.IsActive,
                                         Id = o.Id
                                     };

                var totalCount = await filteredActivecurrency.CountAsync();

                var dbList = await activecurrency.ToListAsync();
                var results = new List<GetActivecurrencyForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetActivecurrencyForViewDto()
                    {
                        Activecurrency = new ActivecurrencyDto
                        {

                            Entity = o.Entity,
                            Currency = o.Currency,
                            AlphabeticCode = o.AlphabeticCode,
                            NumericCode = o.NumericCode,
                            MinorUnit = o.MinorUnit,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetActivecurrencyForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetActivecurrencyForViewDto> GetActivecurrencyForView(int id)
        {
            var activecurrency = await _activecurrencyRepository.GetAsync(id);

            var output = new GetActivecurrencyForViewDto { Activecurrency = ObjectMapper.Map<ActivecurrencyDto>(activecurrency) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Activecurrency_Edit)]
        public async Task<GetActivecurrencyForEditOutput> GetActivecurrencyForEdit(EntityDto input)
        {
            var activecurrency = await _activecurrencyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetActivecurrencyForEditOutput { Activecurrency = ObjectMapper.Map<CreateOrEditActivecurrencyDto>(activecurrency) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditActivecurrencyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Activecurrency_Create)]
        protected virtual async Task Create(CreateOrEditActivecurrencyDto input)
        {
            var activecurrency = ObjectMapper.Map<Activecurrency>(input);
            activecurrency.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                activecurrency.TenantId = (int?)AbpSession.TenantId;
            }

            await _activecurrencyRepository.InsertAsync(activecurrency);

        }

        [AbpAuthorize(AppPermissions.Pages_Activecurrency_Edit)]
        protected virtual async Task Update(CreateOrEditActivecurrencyDto input)
        {
            var activecurrency = await _activecurrencyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, activecurrency);

        }

        [AbpAuthorize(AppPermissions.Pages_Activecurrency_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _activecurrencyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetActivecurrencyToExcel(GetAllActivecurrencyForExcelInput input)
        {

            var filteredActivecurrency = _activecurrencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Entity.Contains(input.Filter) || e.Currency.Contains(input.Filter) || e.AlphabeticCode.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.MinorUnit.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EntityFilter), e => e.Entity.Contains(input.EntityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyFilter), e => e.Currency.Contains(input.CurrencyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AlphabeticCodeFilter), e => e.AlphabeticCode.Contains(input.AlphabeticCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinorUnitFilter), e => e.MinorUnit.Contains(input.MinorUnitFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredActivecurrency
                         select new GetActivecurrencyForViewDto()
                         {
                             Activecurrency = new ActivecurrencyDto
                             {
                                 Entity = o.Entity,
                                 Currency = o.Currency,
                                 AlphabeticCode = o.AlphabeticCode,
                                 NumericCode = o.NumericCode,
                                 MinorUnit = o.MinorUnit,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var activecurrencyListDtos = await query.ToListAsync();

            return _activecurrencyExcelExporter.ExportToFile(activecurrencyListDtos);
        }

    }
}