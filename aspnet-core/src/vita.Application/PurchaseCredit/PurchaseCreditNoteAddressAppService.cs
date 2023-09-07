using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseCredit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseCredit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteAddress)]
    public class PurchaseCreditNoteAddressAppService : vitaAppServiceBase, IPurchaseCreditNoteAddressAppService
    {
        private readonly IRepository<PurchaseCreditNoteAddress, long> _purchaseCreditNoteAddressRepository;

        public PurchaseCreditNoteAddressAppService(IRepository<PurchaseCreditNoteAddress, long> purchaseCreditNoteAddressRepository)
        {
            _purchaseCreditNoteAddressRepository = purchaseCreditNoteAddressRepository;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNoteAddressForViewDto>> GetAll(GetAllPurchaseCreditNoteAddressInput input)
        {

            var filteredPurchaseCreditNoteAddress = _purchaseCreditNoteAddressRepository.GetAll()
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

            var pagedAndFilteredPurchaseCreditNoteAddress = filteredPurchaseCreditNoteAddress
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNoteAddress = from o in pagedAndFilteredPurchaseCreditNoteAddress
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

            var totalCount = await filteredPurchaseCreditNoteAddress.CountAsync();

            var dbList = await purchaseCreditNoteAddress.ToListAsync();
            var results = new List<GetPurchaseCreditNoteAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNoteAddressForViewDto()
                {
                    PurchaseCreditNoteAddress = new PurchaseCreditNoteAddressDto
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

            return new PagedResultDto<GetPurchaseCreditNoteAddressForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNoteAddressForViewDto> GetPurchaseCreditNoteAddressForView(long id)
        {
            var purchaseCreditNoteAddress = await _purchaseCreditNoteAddressRepository.GetAsync(id);

            var output = new GetPurchaseCreditNoteAddressForViewDto { PurchaseCreditNoteAddress = ObjectMapper.Map<PurchaseCreditNoteAddressDto>(purchaseCreditNoteAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteAddress_Edit)]
        public async Task<GetPurchaseCreditNoteAddressForEditOutput> GetPurchaseCreditNoteAddressForEdit(EntityDto<long> input)
        {
            var purchaseCreditNoteAddress = await _purchaseCreditNoteAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNoteAddressForEditOutput { PurchaseCreditNoteAddress = ObjectMapper.Map<CreateOrEditPurchaseCreditNoteAddressDto>(purchaseCreditNoteAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNoteAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteAddress_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNoteAddressDto input)
        {
            var purchaseCreditNoteAddress = ObjectMapper.Map<PurchaseCreditNoteAddress>(input);
            purchaseCreditNoteAddress.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseCreditNoteAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNoteAddressRepository.InsertAsync(purchaseCreditNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteAddress_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNoteAddressDto input)
        {
            var purchaseCreditNoteAddress = await _purchaseCreditNoteAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteAddress_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNoteAddressRepository.DeleteAsync(input.Id);
        }

    }
}