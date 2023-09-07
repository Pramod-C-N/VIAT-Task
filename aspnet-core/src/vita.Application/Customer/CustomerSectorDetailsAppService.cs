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
    [AbpAuthorize(AppPermissions.Pages_CustomerSectorDetails)]
    public class CustomerSectorDetailsAppService : vitaAppServiceBase, ICustomerSectorDetailsAppService
    {
        private readonly IRepository<CustomerSectorDetail, long> _customerSectorDetailRepository;

        public CustomerSectorDetailsAppService(IRepository<CustomerSectorDetail, long> customerSectorDetailRepository)
        {
            _customerSectorDetailRepository = customerSectorDetailRepository;

        }

        public async Task<PagedResultDto<GetCustomerSectorDetailForViewDto>> GetAll(GetAllCustomerSectorDetailsInput input)
        {

            var filteredCustomerSectorDetails = _customerSectorDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.SubIndustryCode.Contains(input.Filter) || e.SubIndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryCodeFilter), e => e.SubIndustryCode.Contains(input.SubIndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryNameFilter), e => e.SubIndustryName.Contains(input.SubIndustryNameFilter));

            var pagedAndFilteredCustomerSectorDetails = filteredCustomerSectorDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerSectorDetails = from o in pagedAndFilteredCustomerSectorDetails
                                        select new
                                        {

                                            o.CustomerID,
                                            o.CustomerUniqueIdentifier,
                                            o.SubIndustryCode,
                                            o.SubIndustryName,
                                            Id = o.Id
                                        };

            var totalCount = await filteredCustomerSectorDetails.CountAsync();

            var dbList = await customerSectorDetails.ToListAsync();
            var results = new List<GetCustomerSectorDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerSectorDetailForViewDto()
                {
                    CustomerSectorDetail = new CustomerSectorDetailDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        SubIndustryCode = o.SubIndustryCode,
                        SubIndustryName = o.SubIndustryName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerSectorDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerSectorDetails_Edit)]
        public async Task<GetCustomerSectorDetailForEditOutput> GetCustomerSectorDetailForEdit(EntityDto<long> input)
        {
            var customerSectorDetail = await _customerSectorDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerSectorDetailForEditOutput { CustomerSectorDetail = ObjectMapper.Map<CreateOrEditCustomerSectorDetailDto>(customerSectorDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerSectorDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerSectorDetails_Create)]
        protected virtual async Task Create(CreateOrEditCustomerSectorDetailDto input)
        {
            var customerSectorDetail = ObjectMapper.Map<CustomerSectorDetail>(input);

            if (AbpSession.TenantId != null)
            {
                customerSectorDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerSectorDetailRepository.InsertAsync(customerSectorDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerSectorDetails_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerSectorDetailDto input)
        {
            var customerSectorDetail = await _customerSectorDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerSectorDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerSectorDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerSectorDetailRepository.DeleteAsync(input.Id);
        }

    }
}