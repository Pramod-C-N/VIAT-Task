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
    [AbpAuthorize(AppPermissions.Pages_CustomerContactPersons)]
    public class CustomerContactPersonsAppService : vitaAppServiceBase, ICustomerContactPersonsAppService
    {
        private readonly IRepository<CustomerContactPerson, long> _customerContactPersonRepository;

        public CustomerContactPersonsAppService(IRepository<CustomerContactPerson, long> customerContactPersonRepository)
        {
            _customerContactPersonRepository = customerContactPersonRepository;

        }

        public async Task<PagedResultDto<GetCustomerContactPersonForViewDto>> GetAll(GetAllCustomerContactPersonsInput input)
        {

            var filteredCustomerContactPersons = _customerContactPersonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.EmployeeCode.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.GovtId.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeCodeFilter), e => e.EmployeeCode.Contains(input.EmployeeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovtIdFilter), e => e.GovtId.Contains(input.GovtIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredCustomerContactPersons = filteredCustomerContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerContactPersons = from o in pagedAndFilteredCustomerContactPersons
                                         select new
                                         {

                                             o.CustomerID,
                                             o.CustomerUniqueIdentifier,
                                             o.Name,
                                             o.EmployeeCode,
                                             o.ContactNumber,
                                             o.GovtId,
                                             o.Email,
                                             o.Address,
                                             o.Location,
                                             o.Type,
                                             Id = o.Id
                                         };

            var totalCount = await filteredCustomerContactPersons.CountAsync();

            var dbList = await customerContactPersons.ToListAsync();
            var results = new List<GetCustomerContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerContactPersonForViewDto()
                {
                    CustomerContactPerson = new CustomerContactPersonDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        Name = o.Name,
                        EmployeeCode = o.EmployeeCode,
                        ContactNumber = o.ContactNumber,
                        GovtId = o.GovtId,
                        Email = o.Email,
                        Address = o.Address,
                        Location = o.Location,
                        Type = o.Type,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerContactPersons_Edit)]
        public async Task<GetCustomerContactPersonForEditOutput> GetCustomerContactPersonForEdit(EntityDto<long> input)
        {
            var customerContactPerson = await _customerContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerContactPersonForEditOutput { CustomerContactPerson = ObjectMapper.Map<CreateOrEditCustomerContactPersonDto>(customerContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerContactPersonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditCustomerContactPersonDto input)
        {
            var customerContactPerson = ObjectMapper.Map<CustomerContactPerson>(input);

            if (AbpSession.TenantId != null)
            {
                customerContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerContactPersonRepository.InsertAsync(customerContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerContactPersonDto input)
        {
            var customerContactPerson = await _customerContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerContactPersons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}