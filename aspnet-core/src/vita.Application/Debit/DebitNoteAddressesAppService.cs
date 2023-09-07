using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Debit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Debit
{
    [AbpAuthorize(AppPermissions.Pages_DebitNoteAddresses)]
    public class DebitNoteAddressesAppService : vitaAppServiceBase, IDebitNoteAddressesAppService
    {
        private readonly IRepository<DebitNoteAddress, long> _debitNoteAddressRepository;

        public DebitNoteAddressesAppService(IRepository<DebitNoteAddress, long> debitNoteAddressRepository)
        {
            _debitNoteAddressRepository = debitNoteAddressRepository;

        }

        public async Task<PagedResultDto<GetDebitNoteAddressForViewDto>> GetAll(GetAllDebitNoteAddressesInput input)
        {

            var filteredDebitNoteAddresses = _debitNoteAddressRepository.GetAll()
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

            var pagedAndFilteredDebitNoteAddresses = filteredDebitNoteAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteAddresses = from o in pagedAndFilteredDebitNoteAddresses
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

            var totalCount = await filteredDebitNoteAddresses.CountAsync();

            var dbList = await debitNoteAddresses.ToListAsync();
            var results = new List<GetDebitNoteAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteAddressForViewDto()
                {
                    DebitNoteAddress = new DebitNoteAddressDto
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

            return new PagedResultDto<GetDebitNoteAddressForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteAddresses_Edit)]
        public async Task<GetDebitNoteAddressForEditOutput> GetDebitNoteAddressForEdit(EntityDto<long> input)
        {
            var debitNoteAddress = await _debitNoteAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteAddressForEditOutput { DebitNoteAddress = ObjectMapper.Map<CreateOrEditDebitNoteAddressDto>(debitNoteAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteAddressDto input)
        {
                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteAddresses_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteAddressDto input)
        {
            var debitNoteAddress = ObjectMapper.Map<DebitNoteAddress>(input);
            debitNoteAddress.UniqueIdentifier= Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                debitNoteAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteAddressRepository.InsertAsync(debitNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteAddressDto input)
        {
            var debitNoteAddress = await _debitNoteAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteAddresses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteAddressRepository.DeleteAsync(input.Id);
        }

    }
}