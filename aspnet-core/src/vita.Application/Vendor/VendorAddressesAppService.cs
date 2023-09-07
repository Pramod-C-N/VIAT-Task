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

namespace vita.Vendor
{
    [AbpAuthorize(AppPermissions.Pages_VendorAddresses)]
    public class VendorAddressesAppService : vitaAppServiceBase, IVendorAddressesAppService
    {
        private readonly IRepository<VendorAddress, long> _vendorAddressRepository;

        public VendorAddressesAppService(IRepository<VendorAddress, long> vendorAddressRepository)
        {
            _vendorAddressRepository = vendorAddressRepository;

        }

        public async Task<PagedResultDto<GetVendorAddressForViewDto>> GetAll(GetAllVendorAddressesInput input)
        {

            var filteredVendorAddresses = _vendorAddressRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.Street.Contains(input.Filter) || e.AdditionalStreet.Contains(input.Filter) || e.BuildingNo.Contains(input.Filter) || e.AdditionalNo.Contains(input.Filter) || e.City.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Neighbourhood.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StreetFilter), e => e.Street.Contains(input.StreetFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalStreetFilter), e => e.AdditionalStreet.Contains(input.AdditionalStreetFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuildingNoFilter), e => e.BuildingNo.Contains(input.BuildingNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalNoFilter), e => e.AdditionalNo.Contains(input.AdditionalNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode.Contains(input.PostalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State.Contains(input.StateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NeighbourhoodFilter), e => e.Neighbourhood.Contains(input.NeighbourhoodFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode.Contains(input.CountryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredVendorAddresses = filteredVendorAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorAddresses = from o in pagedAndFilteredVendorAddresses
                                  select new
                                  {

                                      o.VendorID,
                                      o.VendorUniqueIdentifier,
                                      o.Street,
                                      o.AdditionalStreet,
                                      o.BuildingNo,
                                      o.AdditionalNo,
                                      o.City,
                                      o.PostalCode,
                                      o.State,
                                      o.Neighbourhood,
                                      o.CountryCode,
                                      o.Type,
                                      Id = o.Id
                                  };

            var totalCount = await filteredVendorAddresses.CountAsync();

            var dbList = await vendorAddresses.ToListAsync();
            var results = new List<GetVendorAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorAddressForViewDto()
                {
                    VendorAddress = new VendorAddressDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        Street = o.Street,
                        AdditionalStreet = o.AdditionalStreet,
                        BuildingNo = o.BuildingNo,
                        AdditionalNo = o.AdditionalNo,
                        City = o.City,
                        PostalCode = o.PostalCode,
                        State = o.State,
                        Neighbourhood = o.Neighbourhood,
                        CountryCode = o.CountryCode,
                        Type = o.Type,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorAddressForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorAddresses_Edit)]
        public async Task<GetVendorAddressForEditOutput> GetVendorAddressForEdit(EntityDto<long> input)
        {
            var vendorAddress = await _vendorAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorAddressForEditOutput { VendorAddress = ObjectMapper.Map<CreateOrEditVendorAddressDto>(vendorAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorAddresses_Create)]
        protected virtual async Task Create(CreateOrEditVendorAddressDto input)
        {
            var vendorAddress = ObjectMapper.Map<VendorAddress>(input);

            if (AbpSession.TenantId != null)
            {
                vendorAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorAddressRepository.InsertAsync(vendorAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditVendorAddressDto input)
        {
            var vendorAddress = await _vendorAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorAddresses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorAddressRepository.DeleteAsync(input.Id);
        }

    }
}