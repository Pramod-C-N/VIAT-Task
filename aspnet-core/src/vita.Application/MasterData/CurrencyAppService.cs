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
using vita.EntityFrameworkCore;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_Currency)]
    public class CurrencyAppService : vitaAppServiceBase, ICurrencyAppService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly ICurrencyExcelExporter _currencyExcelExporter;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;

        public CurrencyAppService(IRepository<Currency> currencyRepository, ICurrencyExcelExporter currencyExcelExporter, IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _currencyRepository = currencyRepository;
            _currencyExcelExporter = currencyExcelExporter;
            _dbContextProvider = dbContextProvider;

        }



        public async Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrencyInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredCurrency = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.MinorUnit.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinorUnitFilter), e => e.MinorUnit.Contains(input.MinorUnitFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.Contains(input.CountryFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredCurrency = filteredCurrency
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var currency = from o in pagedAndFilteredCurrency
                               select new
                               {

                                   o.Name,
                                   o.Description,
                                   o.Code,
                                   o.NumericCode,
                                   o.MinorUnit,
                                   o.Country,
                                   o.IsActive,
                                   Id = o.Id
                               };

                var totalCount = await filteredCurrency.CountAsync();

                var dbList = await currency.ToListAsync();
                var results = new List<GetCurrencyForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCurrencyForViewDto()
                    {
                        Currency = new CurrencyDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            NumericCode = o.NumericCode,
                            MinorUnit = o.MinorUnit,
                            Country = o.Country,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetCurrencyForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        public async Task<GetCurrencyForViewDto> GetCurrencyForView(int id)
        {
            var currency = await _currencyRepository.GetAsync(id);

            var output = new GetCurrencyForViewDto { Currency = ObjectMapper.Map<CurrencyDto>(currency) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Currency_Edit)]
        public async Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCurrencyForEditOutput { Currency = ObjectMapper.Map<CreateOrEditCurrencyDto>(currency) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCurrencyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Currency_Create)]
        protected virtual async Task Create(CreateOrEditCurrencyDto input)
        {
            var currency = ObjectMapper.Map<Currency>(input);
            currency.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                currency.TenantId = (int?)AbpSession.TenantId;
            }

            await _currencyRepository.InsertAsync(currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currency_Edit)]
        protected virtual async Task Update(CreateOrEditCurrencyDto input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currency_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _currencyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCurrencyToExcel(GetAllCurrencyForExcelInput input)
        {

            var filteredCurrency = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.MinorUnit.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinorUnitFilter), e => e.MinorUnit.Contains(input.MinorUnitFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.Contains(input.CountryFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredCurrency
                         select new GetCurrencyForViewDto()
                         {
                             Currency = new CurrencyDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 NumericCode = o.NumericCode,
                                 MinorUnit = o.MinorUnit,
                                 Country = o.Country,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var currencyListDtos = await query.ToListAsync();

            return _currencyExcelExporter.ExportToFile(currencyListDtos);
        }

    }
}