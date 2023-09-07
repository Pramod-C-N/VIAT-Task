using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.TenantDetails.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.TenantDetails
{
    [AbpAuthorize(AppPermissions.Pages_TenantDocuments)]
    public class TenantDocumentsAppService : vitaAppServiceBase, ITenantDocumentsAppService
    {
        private readonly IRepository<TenantDocuments> _tenantDocumentsRepository;

        public TenantDocumentsAppService(IRepository<TenantDocuments> tenantDocumentsRepository)
        {
            _tenantDocumentsRepository = tenantDocumentsRepository;

        }

        public async Task<PagedResultDto<GetTenantDocumentsForViewDto>> GetAll(GetAllTenantDocumentsInput input)
        {

            var filteredTenantDocuments = _tenantDocumentsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BranchId.Contains(input.Filter) || e.BranchName.Contains(input.Filter) || e.DocumentType.Contains(input.Filter) || e.DocumentId.Contains(input.Filter) || e.DocumentNumber.Contains(input.Filter) || e.DocumentPath.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BranchIdFilter), e => e.BranchId.Contains(input.BranchIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNameFilter), e => e.BranchName.Contains(input.BranchNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeFilter), e => e.DocumentType.Contains(input.DocumentTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentIdFilter), e => e.DocumentId.Contains(input.DocumentIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNumberFilter), e => e.DocumentNumber.Contains(input.DocumentNumberFilter))
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentPathFilter), e => e.DocumentPath.Contains(input.DocumentPathFilter));

            var pagedAndFilteredTenantDocuments = filteredTenantDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantDocuments = from o in pagedAndFilteredTenantDocuments
                                  select new
                                  {

                                      o.BranchId,
                                      o.BranchName,
                                      o.DocumentType,
                                      o.DocumentId,
                                      o.DocumentNumber,
                                      o.RegistrationDate,
                                      o.DocumentPath,
                                      Id = o.Id
                                  };

            var totalCount = await filteredTenantDocuments.CountAsync();

            var dbList = await tenantDocuments.ToListAsync();
            var results = new List<GetTenantDocumentsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantDocumentsForViewDto()
                {
                    TenantDocuments = new TenantDocumentsDto
                    {

                        BranchId = o.BranchId,
                        BranchName = o.BranchName,
                        DocumentType = o.DocumentType,
                        DocumentId = o.DocumentId,
                        DocumentNumber = o.DocumentNumber,
                        RegistrationDate = o.RegistrationDate,
                        DocumentPath = o.DocumentPath,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantDocumentsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Edit)]
        public async Task<GetTenantDocumentsForEditOutput> GetTenantDocumentsForEdit(EntityDto input)
        {
            var tenantDocuments = await _tenantDocumentsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantDocumentsForEditOutput { TenantDocuments = ObjectMapper.Map<CreateOrEditTenantDocumentsDto>(tenantDocuments) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantDocumentsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Create)]
        protected virtual async Task Create(CreateOrEditTenantDocumentsDto input)
        {
            var tenantDocuments = ObjectMapper.Map<TenantDocuments>(input);

            if (AbpSession.TenantId != null)
            {
                tenantDocuments.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantDocumentsRepository.InsertAsync(tenantDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditTenantDocumentsDto input)
        {
            var tenantDocuments = await _tenantDocumentsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantDocumentsRepository.DeleteAsync(input.Id);
        }

    }
}