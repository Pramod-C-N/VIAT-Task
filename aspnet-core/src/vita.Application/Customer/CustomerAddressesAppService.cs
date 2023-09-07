using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Customer.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Customer
{
    [AbpAuthorize(AppPermissions.Pages_CustomerAddresses)]
    public class CustomerAddressesAppService : vitaAppServiceBase, ICustomerAddressesAppService
    {
        private readonly IRepository<CustomerAddress, long> _customerAddressRepository;

        public CustomerAddressesAppService(IRepository<CustomerAddress, long> customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;

        }

        public async Task<PagedResultDto<GetCustomerAddressForViewDto>> GetAll(GetAllCustomerAddressesInput input)
        {

            var filteredCustomerAddresses = _customerAddressRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.Street.Contains(input.Filter) || e.AdditionalStreet.Contains(input.Filter) || e.BuildingNo.Contains(input.Filter) || e.AdditionalNo.Contains(input.Filter) || e.City.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Neighbourhood.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
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

            var pagedAndFilteredCustomerAddresses = filteredCustomerAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerAddresses = from o in pagedAndFilteredCustomerAddresses
                                    select new
                                    {

                                        o.CustomerID,
                                        o.CustomerUniqueIdentifier,
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

            var totalCount = await filteredCustomerAddresses.CountAsync();

            var dbList = await customerAddresses.ToListAsync();
            var results = new List<GetCustomerAddressForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerAddressForViewDto()
                {
                    CustomerAddress = new CustomerAddressDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
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

            return new PagedResultDto<GetCustomerAddressForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerAddresses_Edit)]
        public async Task<GetCustomerAddressForEditOutput> GetCustomerAddressForEdit(EntityDto<long> input)
        {
            var customerAddress = await _customerAddressRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerAddressForEditOutput { CustomerAddress = ObjectMapper.Map<CreateOrEditCustomerAddressDto>(customerAddress) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerAddressDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerAddresses_Create)]
        protected virtual async Task Create(CreateOrEditCustomerAddressDto input)
        {
            var customerAddress = ObjectMapper.Map<CustomerAddress>(input);

            if (AbpSession.TenantId != null)
            {
                customerAddress.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerAddressRepository.InsertAsync(customerAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerAddresses_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerAddressDto input)
        {
            var customerAddress = await _customerAddressRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerAddress);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerAddresses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerAddressRepository.DeleteAsync(input.Id);
        }

    }
}