using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Credit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Credit
{
    [AbpAuthorize(AppPermissions.Pages_CreditNoteAddress)]
    public class CreditNoteAddressAppService : vitaAppServiceBase, ICreditNoteAddressAppService
    {
        private readonly IRepository<CreditNoteAddress, long> _creditNoteAddressRepository;

        public CreditNoteAddressAppService(IRepository<CreditNoteAddress, long> creditNoteAddressRepository)
        {
            _creditNoteAddressRepository = creditNoteAddressRepository;

        }

        public async Task<PagedResultDto<GetCreditNoteAddressForViewDto>> GetAll(GetAllCreditNoteAddressInput input)
        {

            var filteredCreditNoteAddress = _creditNoteAddressRepository.GetAll()
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

            var pagedAndFilteredCreditNoteAddress = filteredCreditNoteAddress
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteAddress = from o in pagedAndFilteredCreditNoteAddress
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

            var totalCount = await filteredCreditNoteAddress.CountAsync();

            var dbList = await creditNoteAddress.ToListAsync();
            var results = new List<GetCreditNoteAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteAddressForViewDto()
                {
                    CreditNoteAddress = new CreditNoteAddressDto
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

            return new PagedResultDto<GetCreditNoteAddressForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCreditNoteAddressForViewDto> GetCreditNoteAddressForView(long id)
        {
            var creditNoteAddress = await _creditNoteAddressRepository.GetAsync(id);

            var output = new GetCreditNoteAddressForViewDto { CreditNoteAddress = ObjectMapper.Map<CreditNoteAddressDto>(creditNoteAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteAddress_Edit)]
        public async Task<GetCreditNoteAddressForEditOutput> GetCreditNoteAddressForEdit(EntityDto<long> input)
        {
            var creditNoteAddress = await _creditNoteAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteAddressForEditOutput { CreditNoteAddress = ObjectMapper.Map<CreateOrEditCreditNoteAddressDto>(creditNoteAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNoteAddressDto input)
        {
                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteAddress_Create)]
        protected virtual async Task Create(CreateOrEditCreditNoteAddressDto input)
        {
            var creditNoteAddress = ObjectMapper.Map<CreditNoteAddress>(input);
            creditNoteAddress.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                creditNoteAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNoteAddressRepository.InsertAsync(creditNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteAddress_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteAddressDto input)
        {
            var creditNoteAddress = await _creditNoteAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteAddress_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNoteAddressRepository.DeleteAsync(input.Id);
        }

    }
}