using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseDebit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseDebit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteAddress)]
    public class PurchaseDebitNoteAddressAppService : vitaAppServiceBase, IPurchaseDebitNoteAddressAppService
    {
        private readonly IRepository<PurchaseDebitNoteAddress, long> _purchaseDebitNoteAddressRepository;

        public PurchaseDebitNoteAddressAppService(IRepository<PurchaseDebitNoteAddress, long> purchaseDebitNoteAddressRepository)
        {
            _purchaseDebitNoteAddressRepository = purchaseDebitNoteAddressRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNoteAddressForViewDto>> GetAll(GetAllPurchaseDebitNoteAddressInput input)
        {

            var filteredPurchaseDebitNoteAddress = _purchaseDebitNoteAddressRepository.GetAll()
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

            var pagedAndFilteredPurchaseDebitNoteAddress = filteredPurchaseDebitNoteAddress
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteAddress = from o in pagedAndFilteredPurchaseDebitNoteAddress
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

            var totalCount = await filteredPurchaseDebitNoteAddress.CountAsync();

            var dbList = await purchaseDebitNoteAddress.ToListAsync();
            var results = new List<GetPurchaseDebitNoteAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNoteAddressForViewDto()
                {
                    PurchaseDebitNoteAddress = new PurchaseDebitNoteAddressDto
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

            return new PagedResultDto<GetPurchaseDebitNoteAddressForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseDebitNoteAddressForViewDto> GetPurchaseDebitNoteAddressForView(long id)
        {
            var purchaseDebitNoteAddress = await _purchaseDebitNoteAddressRepository.GetAsync(id);

            var output = new GetPurchaseDebitNoteAddressForViewDto { PurchaseDebitNoteAddress = ObjectMapper.Map<PurchaseDebitNoteAddressDto>(purchaseDebitNoteAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteAddress_Edit)]
        public async Task<GetPurchaseDebitNoteAddressForEditOutput> GetPurchaseDebitNoteAddressForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteAddress = await _purchaseDebitNoteAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNoteAddressForEditOutput { PurchaseDebitNoteAddress = ObjectMapper.Map<CreateOrEditPurchaseDebitNoteAddressDto>(purchaseDebitNoteAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNoteAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteAddress_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNoteAddressDto input)
        {
            var purchaseDebitNoteAddress = ObjectMapper.Map<PurchaseDebitNoteAddress>(input);
            purchaseDebitNoteAddress.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNoteAddressRepository.InsertAsync(purchaseDebitNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteAddress_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNoteAddressDto input)
        {
            var purchaseDebitNoteAddress = await _purchaseDebitNoteAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteAddress_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNoteAddressRepository.DeleteAsync(input.Id);
        }

    }
}