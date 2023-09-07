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
    [AbpAuthorize(AppPermissions.Pages_CustomerTaxDetailses)]
    public class CustomerTaxDetailsesAppService : vitaAppServiceBase, ICustomerTaxDetailsesAppService
    {
        private readonly IRepository<CustomerTaxDetails, long> _customerTaxDetailsRepository;

        public CustomerTaxDetailsesAppService(IRepository<CustomerTaxDetails, long> customerTaxDetailsRepository)
        {
            _customerTaxDetailsRepository = customerTaxDetailsRepository;

        }

        public async Task<PagedResultDto<GetCustomerTaxDetailsForViewDto>> GetAll(GetAllCustomerTaxDetailsesInput input)
        {

            var filteredCustomerTaxDetailses = _customerTaxDetailsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.BusinessCategory.Contains(input.Filter) || e.OperatingModel.Contains(input.Filter) || e.BusinessSupplies.Contains(input.Filter) || e.SalesVATCategory.Contains(input.Filter) || e.InvoiceType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessCategoryFilter), e => e.BusinessCategory.Contains(input.BusinessCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OperatingModelFilter), e => e.OperatingModel.Contains(input.OperatingModelFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessSuppliesFilter), e => e.BusinessSupplies.Contains(input.BusinessSuppliesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalesVATCategoryFilter), e => e.SalesVATCategory.Contains(input.SalesVATCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceTypeFilter), e => e.InvoiceType.Contains(input.InvoiceTypeFilter));

            var pagedAndFilteredCustomerTaxDetailses = filteredCustomerTaxDetailses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerTaxDetailses = from o in pagedAndFilteredCustomerTaxDetailses
                                       select new
                                       {

                                           o.CustomerID,
                                           o.CustomerUniqueIdentifier,
                                           o.BusinessCategory,
                                           o.OperatingModel,
                                           o.BusinessSupplies,
                                           o.SalesVATCategory,
                                           o.InvoiceType,
                                           Id = o.Id
                                       };

            var totalCount = await filteredCustomerTaxDetailses.CountAsync();

            var dbList = await customerTaxDetailses.ToListAsync();
            var results = new List<GetCustomerTaxDetailsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerTaxDetailsForViewDto()
                {
                    CustomerTaxDetails = new CustomerTaxDetailsDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        BusinessCategory = o.BusinessCategory,
                        OperatingModel = o.OperatingModel,
                        BusinessSupplies = o.BusinessSupplies,
                        SalesVATCategory = o.SalesVATCategory,
                        InvoiceType = o.InvoiceType,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerTaxDetailsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerTaxDetailses_Edit)]
        public async Task<GetCustomerTaxDetailsForEditOutput> GetCustomerTaxDetailsForEdit(EntityDto<long> input)
        {
            var customerTaxDetails = await _customerTaxDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerTaxDetailsForEditOutput { CustomerTaxDetails = ObjectMapper.Map<CreateOrEditCustomerTaxDetailsDto>(customerTaxDetails) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerTaxDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerTaxDetailses_Create)]
        protected virtual async Task Create(CreateOrEditCustomerTaxDetailsDto input)
        {
            var customerTaxDetails = ObjectMapper.Map<CustomerTaxDetails>(input);

            if (AbpSession.TenantId != null)
            {
                customerTaxDetails.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerTaxDetailsRepository.InsertAsync(customerTaxDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerTaxDetailses_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerTaxDetailsDto input)
        {
            var customerTaxDetails = await _customerTaxDetailsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerTaxDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerTaxDetailses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerTaxDetailsRepository.DeleteAsync(input.Id);
        }

    }
}