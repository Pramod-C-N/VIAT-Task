using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Purchase.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryAddresses)]
    public class PurchaseEntryAddressesAppService : vitaAppServiceBase, IPurchaseEntryAddressesAppService
    {
        private readonly IRepository<PurchaseEntryAddress, long> _purchaseEntryAddressRepository;

        public PurchaseEntryAddressesAppService(IRepository<PurchaseEntryAddress, long> purchaseEntryAddressRepository)
        {
            _purchaseEntryAddressRepository = purchaseEntryAddressRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryAddressForViewDto>> GetAll(GetAllPurchaseEntryAddressesInput input)
        {

            var filteredPurchaseEntryAddresses = _purchaseEntryAddressRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Street.Contains(input.Filter) || e.AdditionalStreet.Contains(input.Filter) || e.BuildingNo.Contains(input.Filter) || e.AdditionalNo.Contains(input.Filter) || e.City.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Neighbourhood.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
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

            var pagedAndFilteredPurchaseEntryAddresses = filteredPurchaseEntryAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryAddresses = from o in pagedAndFilteredPurchaseEntryAddresses
                                         select new
                                         {

                                             o.IRNNo,
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

            var totalCount = await filteredPurchaseEntryAddresses.CountAsync();

            var dbList = await purchaseEntryAddresses.ToListAsync();
            var results = new List<GetPurchaseEntryAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryAddressForViewDto()
                {
                    PurchaseEntryAddress = new PurchaseEntryAddressDto
                    {

                        IRNNo = o.IRNNo,
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

            return new PagedResultDto<GetPurchaseEntryAddressForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryAddresses_Edit)]
        public async Task<GetPurchaseEntryAddressForEditOutput> GetPurchaseEntryAddressForEdit(EntityDto<long> input)
        {
            var purchaseEntryAddress = await _purchaseEntryAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryAddressForEditOutput { PurchaseEntryAddress = ObjectMapper.Map<CreateOrEditPurchaseEntryAddressDto>(purchaseEntryAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryAddressDto input)
        {
                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryAddresses_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryAddressDto input)
        {
            var purchaseEntryAddress = ObjectMapper.Map<PurchaseEntryAddress>(input);
            purchaseEntryAddress.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryAddressRepository.InsertAsync(purchaseEntryAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryAddressDto input)
        {
            var purchaseEntryAddress = await _purchaseEntryAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryAddresses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryAddressRepository.DeleteAsync(input.Id);
        }

    }
}