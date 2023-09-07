using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Sales
{
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceAddresses)]
    public class SalesInvoiceAddressesAppService : vitaAppServiceBase, ISalesInvoiceAddressesAppService
    {
        private readonly IRepository<SalesInvoiceAddress, long> _salesInvoiceAddressRepository;

        public SalesInvoiceAddressesAppService(IRepository<SalesInvoiceAddress, long> salesInvoiceAddressRepository)
        {
            _salesInvoiceAddressRepository = salesInvoiceAddressRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoiceAddressForViewDto>> GetAll(GetAllSalesInvoiceAddressesInput input)
        {

            var filteredSalesInvoiceAddresses = _salesInvoiceAddressRepository.GetAll()
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

            var pagedAndFilteredSalesInvoiceAddresses = filteredSalesInvoiceAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceAddresses = from o in pagedAndFilteredSalesInvoiceAddresses
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

            var totalCount = await filteredSalesInvoiceAddresses.CountAsync();

            var dbList = await salesInvoiceAddresses.ToListAsync();
            var results = new List<GetSalesInvoiceAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceAddressForViewDto()
                {
                    SalesInvoiceAddress = new SalesInvoiceAddressDto
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

            return new PagedResultDto<GetSalesInvoiceAddressForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoiceAddressForViewDto> GetSalesInvoiceAddressForView(long id)
        {
            var salesInvoiceAddress = await _salesInvoiceAddressRepository.GetAsync(id);

            var output = new GetSalesInvoiceAddressForViewDto { SalesInvoiceAddress = ObjectMapper.Map<SalesInvoiceAddressDto>(salesInvoiceAddress) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceAddresses_Edit)]
        public async Task<GetSalesInvoiceAddressForEditOutput> GetSalesInvoiceAddressForEdit(EntityDto<long> input)
        {
            var salesInvoiceAddress = await _salesInvoiceAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceAddressForEditOutput { SalesInvoiceAddress = ObjectMapper.Map<CreateOrEditSalesInvoiceAddressDto>(salesInvoiceAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceAddressDto input)
        {
                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceAddresses_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceAddressDto input)
        {
            var salesInvoiceAddress = ObjectMapper.Map<SalesInvoiceAddress>(input);
            salesInvoiceAddress.UniqueIdentifier= Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                salesInvoiceAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoiceAddressRepository.InsertAsync(salesInvoiceAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceAddressDto input)
        {
            var salesInvoiceAddress = await _salesInvoiceAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceAddresses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceAddressRepository.DeleteAsync(input.Id);
        }

    }
}