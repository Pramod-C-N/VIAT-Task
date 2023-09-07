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
    [AbpAuthorize(AppPermissions.Pages_CustomerForeignEntities)]
    public class CustomerForeignEntitiesAppService : vitaAppServiceBase, ICustomerForeignEntitiesAppService
    {
        private readonly IRepository<CustomerForeignEntity, long> _customerForeignEntityRepository;

        public CustomerForeignEntitiesAppService(IRepository<CustomerForeignEntity, long> customerForeignEntityRepository)
        {
            _customerForeignEntityRepository = customerForeignEntityRepository;

        }

        public async Task<PagedResultDto<GetCustomerForeignEntityForViewDto>> GetAll(GetAllCustomerForeignEntitiesInput input)
        {

            var filteredCustomerForeignEntities = _customerForeignEntityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.ForeignEntityName.Contains(input.Filter) || e.ForeignEntityAddress.Contains(input.Filter) || e.LegalRepresentative.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ForeignEntityNameFilter), e => e.ForeignEntityName.Contains(input.ForeignEntityNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ForeignEntityAddressFilter), e => e.ForeignEntityAddress.Contains(input.ForeignEntityAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalRepresentativeFilter), e => e.LegalRepresentative.Contains(input.LegalRepresentativeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.Contains(input.CountryFilter));

            var pagedAndFilteredCustomerForeignEntities = filteredCustomerForeignEntities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerForeignEntities = from o in pagedAndFilteredCustomerForeignEntities
                                          select new
                                          {

                                              o.CustomerID,
                                              o.CustomerUniqueIdentifier,
                                              o.ForeignEntityName,
                                              o.ForeignEntityAddress,
                                              o.LegalRepresentative,
                                              o.Country,
                                              Id = o.Id
                                          };

            var totalCount = await filteredCustomerForeignEntities.CountAsync();

            var dbList = await customerForeignEntities.ToListAsync();
            var results = new List<GetCustomerForeignEntityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerForeignEntityForViewDto()
                {
                    CustomerForeignEntity = new CustomerForeignEntityDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        ForeignEntityName = o.ForeignEntityName,
                        ForeignEntityAddress = o.ForeignEntityAddress,
                        LegalRepresentative = o.LegalRepresentative,
                        Country = o.Country,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerForeignEntityForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerForeignEntities_Edit)]
        public async Task<GetCustomerForeignEntityForEditOutput> GetCustomerForeignEntityForEdit(EntityDto<long> input)
        {
            var customerForeignEntity = await _customerForeignEntityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerForeignEntityForEditOutput { CustomerForeignEntity = ObjectMapper.Map<CreateOrEditCustomerForeignEntityDto>(customerForeignEntity) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerForeignEntityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerForeignEntities_Create)]
        protected virtual async Task Create(CreateOrEditCustomerForeignEntityDto input)
        {
            var customerForeignEntity = ObjectMapper.Map<CustomerForeignEntity>(input);

            if (AbpSession.TenantId != null)
            {
                customerForeignEntity.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerForeignEntityRepository.InsertAsync(customerForeignEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerForeignEntities_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerForeignEntityDto input)
        {
            var customerForeignEntity = await _customerForeignEntityRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerForeignEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerForeignEntities_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerForeignEntityRepository.DeleteAsync(input.Id);
        }

    }
}