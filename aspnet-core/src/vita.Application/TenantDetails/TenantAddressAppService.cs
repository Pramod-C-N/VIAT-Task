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

namespace vita.TenantDetails
{
    [AbpAuthorize(AppPermissions.Pages_TenantAddress)]
    public class TenantAddressAppService : vitaAppServiceBase, ITenantAddressAppService
    {
        private readonly IRepository<TenantAddress> _tenantAddressRepository;

        public TenantAddressAppService(IRepository<TenantAddress> tenantAddressRepository)
        {
            _tenantAddressRepository = tenantAddressRepository;

        }

        public async Task<PagedResultDto<GetTenantAddressForViewDto>> GetAll(GetAllTenantAddressInput input)
        {

            var filteredTenantAddress = _tenantAddressRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AddressTypeId.Contains(input.Filter) || e.AddressType.Contains(input.Filter) || e.BuildingNo.Contains(input.Filter) || e.AdditionalBuildingNumber.Contains(input.Filter) || e.Street.Contains(input.Filter) || e.AdditionalStreet.Contains(input.Filter) || e.Neighbourhood.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.CountryCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressTypeIdFilter), e => e.AddressTypeId.Contains(input.AddressTypeIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressTypeFilter), e => e.AddressType.Contains(input.AddressTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuildingNoFilter), e => e.BuildingNo.Contains(input.BuildingNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalBuildingNumberFilter), e => e.AdditionalBuildingNumber.Contains(input.AdditionalBuildingNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StreetFilter), e => e.Street.Contains(input.StreetFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalStreetFilter), e => e.AdditionalStreet.Contains(input.AdditionalStreetFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NeighbourhoodFilter), e => e.Neighbourhood.Contains(input.NeighbourhoodFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State.Contains(input.StateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode.Contains(input.PostalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.Contains(input.CountryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode.Contains(input.CountryCodeFilter));

            var pagedAndFilteredTenantAddress = filteredTenantAddress
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantAddress = from o in pagedAndFilteredTenantAddress
                                select new
                                {

                                    o.AddressTypeId,
                                    o.AddressType,
                                    o.BuildingNo,
                                    o.AdditionalBuildingNumber,
                                    o.Street,
                                    o.AdditionalStreet,
                                    o.Neighbourhood,
                                    o.City,
                                    o.State,
                                    o.PostalCode,
                                    o.Country,
                                    o.CountryCode,
                                    Id = o.Id
                                };

            var totalCount = await filteredTenantAddress.CountAsync();

            var dbList = await tenantAddress.ToListAsync();
            var results = new List<GetTenantAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantAddressForViewDto()
                {
                    TenantAddress = new TenantAddressDto
                    {

                        AddressTypeId = o.AddressTypeId,
                        AddressType = o.AddressType,
                        BuildingNo = o.BuildingNo,
                        AdditionalBuildingNumber = o.AdditionalBuildingNumber,
                        Street = o.Street,
                        AdditionalStreet = o.AdditionalStreet,
                        Neighbourhood = o.Neighbourhood,
                        City = o.City,
                        State = o.State,
                        PostalCode = o.PostalCode,
                        Country = o.Country,
                        CountryCode = o.CountryCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantAddressForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTenantAddressForViewDto> GetTenantAddressForView(int id)
        {
            var tenantAddress = await _tenantAddressRepository.GetAsync(id);

            var output = new GetTenantAddressForViewDto { TenantAddress = ObjectMapper.Map<TenantAddressDto>(tenantAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantAddress_Edit)]
        public async Task<GetTenantAddressForEditOutput> GetTenantAddressForEdit(EntityDto input)
        {
            var tenantAddress = await _tenantAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantAddressForEditOutput { TenantAddress = ObjectMapper.Map<CreateOrEditTenantAddressDto>(tenantAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantAddress_Create)]
        protected virtual async Task Create(CreateOrEditTenantAddressDto input)
        {
            var tenantAddress = ObjectMapper.Map<TenantAddress>(input);

            if (AbpSession.TenantId != null)
            {
                tenantAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantAddressRepository.InsertAsync(tenantAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantAddress_Edit)]
        protected virtual async Task Update(CreateOrEditTenantAddressDto input)
        {
            var tenantAddress = await _tenantAddressRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantAddress_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantAddressRepository.DeleteAsync(input.Id);
        }

    }
}