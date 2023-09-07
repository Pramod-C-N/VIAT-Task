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

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_Country)]
    public class CountryAppService : vitaAppServiceBase, ICountryAppService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly ICountryExcelExporter _countryExcelExporter;

        public CountryAppService(IRepository<Country> countryRepository, ICountryExcelExporter countryExcelExporter)
        {
            _countryRepository = countryRepository;
            _countryExcelExporter = countryExcelExporter;

        }
        [AbpAllowAnonymous]
        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountryInput input)
        {

            var filteredCountry = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.StateName.Contains(input.Filter) || e.Sovereignty.Contains(input.Filter) || e.AlphaCode.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.InternetCCTLD.Contains(input.Filter) || e.SubDivisionCode.Contains(input.Filter) || e.Alpha3Code.Contains(input.Filter) || e.CountryGroup.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateName.Contains(input.StateNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SovereigntyFilter), e => e.Sovereignty.Contains(input.SovereigntyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AlphaCodeFilter), e => e.AlphaCode.Contains(input.AlphaCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternetCCTLDFilter), e => e.InternetCCTLD.Contains(input.InternetCCTLDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubDivisionCodeFilter), e => e.SubDivisionCode.Contains(input.SubDivisionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Alpha3CodeFilter), e => e.Alpha3Code.Contains(input.Alpha3CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryGroupFilter), e => e.CountryGroup.Contains(input.CountryGroupFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredCountry = filteredCountry
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var country = from o in pagedAndFilteredCountry
                          select new
                          {

                              o.Name,
                              o.StateName,
                              o.Sovereignty,
                              o.AlphaCode,
                              o.NumericCode,
                              o.InternetCCTLD,
                              o.SubDivisionCode,
                              o.Alpha3Code,
                              o.CountryGroup,
                              o.IsActive,
                              Id = o.Id
                          };

            var totalCount = await filteredCountry.CountAsync();

            var dbList = await country.ToListAsync();
            var results = new List<GetCountryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCountryForViewDto()
                {
                    Country = new CountryDto
                    {

                        Name = o.Name,
                        StateName = o.StateName,
                        Sovereignty = o.Sovereignty,
                        AlphaCode = o.AlphaCode,
                        NumericCode = o.NumericCode,
                        InternetCCTLD = o.InternetCCTLD,
                        SubDivisionCode = o.SubDivisionCode,
                        Alpha3Code = o.Alpha3Code,
                        CountryGroup = o.CountryGroup,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCountryForViewDto> GetCountryForView(int id)
        {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Country_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Country_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);
            country.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                country.TenantId = (int?)AbpSession.TenantId;
            }

            await _countryRepository.InsertAsync(country);

        }

        [AbpAuthorize(AppPermissions.Pages_Country_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, country);

        }

        [AbpAuthorize(AppPermissions.Pages_Country_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountryToExcel(GetAllCountryForExcelInput input)
        {

            var filteredCountry = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.StateName.Contains(input.Filter) || e.Sovereignty.Contains(input.Filter) || e.AlphaCode.Contains(input.Filter) || e.NumericCode.Contains(input.Filter) || e.InternetCCTLD.Contains(input.Filter) || e.SubDivisionCode.Contains(input.Filter) || e.Alpha3Code.Contains(input.Filter) || e.CountryGroup.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateName.Contains(input.StateNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SovereigntyFilter), e => e.Sovereignty.Contains(input.SovereigntyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AlphaCodeFilter), e => e.AlphaCode.Contains(input.AlphaCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumericCodeFilter), e => e.NumericCode.Contains(input.NumericCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternetCCTLDFilter), e => e.InternetCCTLD.Contains(input.InternetCCTLDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubDivisionCodeFilter), e => e.SubDivisionCode.Contains(input.SubDivisionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Alpha3CodeFilter), e => e.Alpha3Code.Contains(input.Alpha3CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryGroupFilter), e => e.CountryGroup.Contains(input.CountryGroupFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredCountry
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 Name = o.Name,
                                 StateName = o.StateName,
                                 Sovereignty = o.Sovereignty,
                                 AlphaCode = o.AlphaCode,
                                 NumericCode = o.NumericCode,
                                 InternetCCTLD = o.InternetCCTLD,
                                 SubDivisionCode = o.SubDivisionCode,
                                 Alpha3Code = o.Alpha3Code,
                                 CountryGroup = o.CountryGroup,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var countryListDtos = await query.ToListAsync();

            return _countryExcelExporter.ExportToFile(countryListDtos);
        }

    }
}