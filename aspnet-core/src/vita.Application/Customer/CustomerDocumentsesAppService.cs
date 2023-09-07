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
    [AbpAuthorize(AppPermissions.Pages_CustomerDocumentses)]
    public class CustomerDocumentsesAppService : vitaAppServiceBase, ICustomerDocumentsesAppService
    {
        private readonly IRepository<CustomerDocuments, long> _customerDocumentsRepository;

        public CustomerDocumentsesAppService(IRepository<CustomerDocuments, long> customerDocumentsRepository)
        {
            _customerDocumentsRepository = customerDocumentsRepository;

        }

        public async Task<PagedResultDto<GetCustomerDocumentsForViewDto>> GetAll(GetAllCustomerDocumentsesInput input)
        {

            var filteredCustomerDocumentses = _customerDocumentsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerID.Contains(input.Filter) || e.DocumentTypeCode.Contains(input.Filter) || e.DocumentName.Contains(input.Filter) || e.DocumentNumber.Contains(input.Filter) || e.Status.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIDFilter), e => e.CustomerID.Contains(input.CustomerIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerUniqueIdentifierFilter.ToString()), e => e.CustomerUniqueIdentifier.ToString() == input.CustomerUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeCodeFilter), e => e.DocumentTypeCode.Contains(input.DocumentTypeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNameFilter), e => e.DocumentName.Contains(input.DocumentNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNumberFilter), e => e.DocumentNumber.Contains(input.DocumentNumberFilter))
                        .WhereIf(input.MinDoumentDateFilter != null, e => e.DoumentDate >= input.MinDoumentDateFilter)
                        .WhereIf(input.MaxDoumentDateFilter != null, e => e.DoumentDate <= input.MaxDoumentDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status.Contains(input.StatusFilter));

            var pagedAndFilteredCustomerDocumentses = filteredCustomerDocumentses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerDocumentses = from o in pagedAndFilteredCustomerDocumentses
                                      select new
                                      {

                                          o.CustomerID,
                                          o.CustomerUniqueIdentifier,
                                          o.DocumentTypeCode,
                                          o.DocumentName,
                                          o.DocumentNumber,
                                          o.DoumentDate,
                                          o.Status,
                                          Id = o.Id
                                      };

            var totalCount = await filteredCustomerDocumentses.CountAsync();

            var dbList = await customerDocumentses.ToListAsync();
            var results = new List<GetCustomerDocumentsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerDocumentsForViewDto()
                {
                    CustomerDocuments = new CustomerDocumentsDto
                    {

                        CustomerID = o.CustomerID,
                        CustomerUniqueIdentifier = o.CustomerUniqueIdentifier,
                        DocumentTypeCode = o.DocumentTypeCode,
                        DocumentName = o.DocumentName,
                        DocumentNumber = o.DocumentNumber,
                        DoumentDate = o.DoumentDate,
                        Status = o.Status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerDocumentsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerDocumentses_Edit)]
        public async Task<GetCustomerDocumentsForEditOutput> GetCustomerDocumentsForEdit(EntityDto<long> input)
        {
            var customerDocuments = await _customerDocumentsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerDocumentsForEditOutput { CustomerDocuments = ObjectMapper.Map<CreateOrEditCustomerDocumentsDto>(customerDocuments) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerDocumentsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerDocumentses_Create)]
        protected virtual async Task Create(CreateOrEditCustomerDocumentsDto input)
        {
            var customerDocuments = ObjectMapper.Map<CustomerDocuments>(input);

            if (AbpSession.TenantId != null)
            {
                customerDocuments.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerDocumentsRepository.InsertAsync(customerDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerDocumentses_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerDocumentsDto input)
        {
            var customerDocuments = await _customerDocumentsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerDocumentses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerDocumentsRepository.DeleteAsync(input.Id);
        }

    }
}