using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.DraftFee.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftAddresses)]
    public class DraftAddressesAppService : vitaAppServiceBase, IDraftAddressesAppService
    {
        private readonly IRepository<DraftAddress, long> _draftAddressRepository;

        public DraftAddressesAppService(IRepository<DraftAddress, long> draftAddressRepository)
        {
            _draftAddressRepository = draftAddressRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftAddressForViewDto>> GetAll(GetAllDraftAddressesInput input)
        {

            var filteredDraftAddresses = _draftAddressRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Street.Contains(input.Filter) || e.AdditionalStreet.Contains(input.Filter) || e.BuildingNo.Contains(input.Filter) || e.AdditionalNo.Contains(input.Filter) || e.City.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Neighbourhood.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.Language.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter));

            var pagedAndFilteredDraftAddresses = filteredDraftAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftAddresses = from o in pagedAndFilteredDraftAddresses
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
                                     o.AdditionalData1,
                                     o.Language,
                                     Id = o.Id
                                 };

            var totalCount = await filteredDraftAddresses.CountAsync();

            var dbList = await draftAddresses.ToListAsync();
            var results = new List<GetDraftAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftAddressForViewDto()
                {
                    DraftAddress = new DraftAddressDto
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
                        AdditionalData1 = o.AdditionalData1,
                        Language = o.Language,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftAddressForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftAddressForViewDto> GetDraftAddressForView(long id)
        {
            var draftAddress = await _draftAddressRepository.GetAsync(id);

            var output = new GetDraftAddressForViewDto { DraftAddress = ObjectMapper.Map<DraftAddressDto>(draftAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftAddresses_Edit)]
        public virtual async Task<GetDraftAddressForEditOutput> GetDraftAddressForEdit(EntityDto<long> input)
        {
            var draftAddress = await _draftAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftAddressForEditOutput { DraftAddress = ObjectMapper.Map<CreateOrEditDraftAddressDto>(draftAddress) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftAddresses_Create)]
        protected virtual async Task Create(CreateOrEditDraftAddressDto input)
        {
            var draftAddress = ObjectMapper.Map<DraftAddress>(input);
            draftAddress.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                draftAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftAddressRepository.InsertAsync(draftAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditDraftAddressDto input)
        {
            var draftAddress = await _draftAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, draftAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftAddresses_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftAddressRepository.DeleteAsync(input.Id);
        }

    }
}