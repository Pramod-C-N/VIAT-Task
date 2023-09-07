using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Exporting;
using vita.MasterData.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_TransactionCategory)]
    public class TransactionCategoryAppService : vitaAppServiceBase, ITransactionCategoryAppService
    {
        private readonly IRepository<TransactionCategory> _transactionCategoryRepository;
        private readonly ITransactionCategoryExcelExporter _transactionCategoryExcelExporter;

        public TransactionCategoryAppService(IRepository<TransactionCategory> transactionCategoryRepository, ITransactionCategoryExcelExporter transactionCategoryExcelExporter)
        {
            _transactionCategoryRepository = transactionCategoryRepository;
            _transactionCategoryExcelExporter = transactionCategoryExcelExporter;

        }

        public async Task<PagedResultDto<GetTransactionCategoryForViewDto>> GetAll(GetAllTransactionCategoryInput input)
        {

            var filteredTransactionCategory = _transactionCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTransactionCategory = filteredTransactionCategory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var transactionCategory = from o in pagedAndFilteredTransactionCategory
                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          o.Code,
                                          o.IsActive,
                                          Id = o.Id
                                      };

            var totalCount = await filteredTransactionCategory.CountAsync();

            var dbList = await transactionCategory.ToListAsync();
            var results = new List<GetTransactionCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTransactionCategoryForViewDto()
                {
                    TransactionCategory = new TransactionCategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Code = o.Code,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTransactionCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTransactionCategoryForViewDto> GetTransactionCategoryForView(int id)
        {
            var transactionCategory = await _transactionCategoryRepository.GetAsync(id);

            var output = new GetTransactionCategoryForViewDto { TransactionCategory = ObjectMapper.Map<TransactionCategoryDto>(transactionCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionCategory_Edit)]
        public async Task<GetTransactionCategoryForEditOutput> GetTransactionCategoryForEdit(EntityDto input)
        {
            var transactionCategory = await _transactionCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionCategoryForEditOutput { TransactionCategory = ObjectMapper.Map<CreateOrEditTransactionCategoryDto>(transactionCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTransactionCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TransactionCategory_Create)]
        protected virtual async Task Create(CreateOrEditTransactionCategoryDto input)
        {
            var transactionCategory = ObjectMapper.Map<TransactionCategory>(input);
            transactionCategory.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                transactionCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _transactionCategoryRepository.InsertAsync(transactionCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TransactionCategory_Edit)]
        protected virtual async Task Update(CreateOrEditTransactionCategoryDto input)
        {
            var transactionCategory = await _transactionCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transactionCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TransactionCategory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _transactionCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTransactionCategoryToExcel(GetAllTransactionCategoryForExcelInput input)
        {

            var filteredTransactionCategory = _transactionCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredTransactionCategory
                         select new GetTransactionCategoryForViewDto()
                         {
                             TransactionCategory = new TransactionCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var transactionCategoryListDtos = await query.ToListAsync();

            return _transactionCategoryExcelExporter.ExportToFile(transactionCategoryListDtos);
        }

    }
}