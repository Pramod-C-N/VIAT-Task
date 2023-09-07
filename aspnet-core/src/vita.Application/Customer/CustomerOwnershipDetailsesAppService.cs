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
    [AbpAuthorize(AppPermissions.Pages_CustomerOwnershipDetailses)]
    public class CustomerOwnershipDetailsesAppService : vitaAppServiceBase, ICustomerOwnershipDetailsesAppService
    {
        private readonly IRepository<CustomerOwnershipDetails, long> _customerOwnershipDetailsRepository;

        public CustomerOwnershipDetailsesAppService(IRepository<CustomerOwnershipDetails, long> customerOwnershipDetailsRepository)
        {
            _customerOwnershipDetailsRepository = customerOwnershipDetailsRepository;

        }

        public async Task<PagedResultDto<GetCustomerOwnershipDetailsForViewDto>> GetAll(GetAllCustomerOwnershipDetailsesInput input)
        {

            var filteredCustomerOwnershipDetailses = _customerOwnershipDetailsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.PartnerName.Contains(input.Filter) || e.PartnerConstitution.Contains(input.Filter) || e.PartnerNationality.Contains(input.Filter) || e.RepresentativeName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerNameFilter), e => e.PartnerName.Contains(input.PartnerNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerConstitutionFilter), e => e.PartnerConstitution.Contains(input.PartnerConstitutionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerNationalityFilter), e => e.PartnerNationality.Contains(input.PartnerNationalityFilter))
                        .WhereIf(input.MinCapitalAmountFilter != null, e => e.CapitalAmount >= input.MinCapitalAmountFilter)
                        .WhereIf(input.MaxCapitalAmountFilter != null, e => e.CapitalAmount <= input.MaxCapitalAmountFilter)
                        .WhereIf(input.MinCapitalShareFilter != null, e => e.CapitalShare >= input.MinCapitalShareFilter)
                        .WhereIf(input.MaxCapitalShareFilter != null, e => e.CapitalShare <= input.MaxCapitalShareFilter)
                        .WhereIf(input.MinProfitShareFilter != null, e => e.ProfitShare >= input.MinProfitShareFilter)
                        .WhereIf(input.MaxProfitShareFilter != null, e => e.ProfitShare <= input.MaxProfitShareFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RepresentativeNameFilter), e => e.RepresentativeName.Contains(input.RepresentativeNameFilter));

            var pagedAndFilteredCustomerOwnershipDetailses = filteredCustomerOwnershipDetailses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerOwnershipDetailses = from o in pagedAndFilteredCustomerOwnershipDetailses
                                             select new
                                             {

                                                 o.CustomerID,
                                                 o.CustomerUniqueIdentifier,
                                                 o.PartnerName,
                                                 o.PartnerConstitution,
                                                 o.PartnerNationality,
                                                 o.CapitalAmount,
                                                 o.CapitalShare,
                                                 o.ProfitShare,
                                                 o.RepresentativeName,
                                                 Id = o.Id
                                             };

            var totalCount = await filteredCustomerOwnershipDetailses.CountAsync();

            var dbList = await customerOwnershipDetailses.ToListAsync();
            var results = new List<GetCustomerOwnershipDetailsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerOwnershipDetailsForViewDto()
                {
                    CustomerOwnershipDetails = new CustomerOwnershipDetailsDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        PartnerName = o.PartnerName,
                        PartnerConstitution = o.PartnerConstitution,
                        PartnerNationality = o.PartnerNationality,
                        CapitalAmount = o.CapitalAmount,
                        CapitalShare = o.CapitalShare,
                        ProfitShare = o.ProfitShare,
                        RepresentativeName = o.RepresentativeName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerOwnershipDetailsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerOwnershipDetailses_Edit)]
        public async Task<GetCustomerOwnershipDetailsForEditOutput> GetCustomerOwnershipDetailsForEdit(EntityDto<long> input)
        {
            var customerOwnershipDetails = await _customerOwnershipDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerOwnershipDetailsForEditOutput { CustomerOwnershipDetails = ObjectMapper.Map<CreateOrEditCustomerOwnershipDetailsDto>(customerOwnershipDetails) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerOwnershipDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerOwnershipDetailses_Create)]
        protected virtual async Task Create(CreateOrEditCustomerOwnershipDetailsDto input)
        {
            var customerOwnershipDetails = ObjectMapper.Map<CustomerOwnershipDetails>(input);

            if (AbpSession.TenantId != null)
            {
                customerOwnershipDetails.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerOwnershipDetailsRepository.InsertAsync(customerOwnershipDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerOwnershipDetailses_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerOwnershipDetailsDto input)
        {
            var customerOwnershipDetails = await _customerOwnershipDetailsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerOwnershipDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerOwnershipDetailses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerOwnershipDetailsRepository.DeleteAsync(input.Id);
        }

    }
}