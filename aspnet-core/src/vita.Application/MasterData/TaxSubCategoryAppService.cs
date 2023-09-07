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
    [AbpAuthorize(AppPermissions.Pages_TaxSubCategory)]
    public class TaxSubCategoryAppService : vitaAppServiceBase, ITaxSubCategoryAppService
    {
        private readonly IRepository<TaxSubCategory> _taxSubCategoryRepository;
        private readonly ITaxSubCategoryExcelExporter _taxSubCategoryExcelExporter;

        public TaxSubCategoryAppService(IRepository<TaxSubCategory> taxSubCategoryRepository, ITaxSubCategoryExcelExporter taxSubCategoryExcelExporter)
        {
            _taxSubCategoryRepository = taxSubCategoryRepository;
            _taxSubCategoryExcelExporter = taxSubCategoryExcelExporter;

        }

        public async Task<PagedResultDto<GetTaxSubCategoryForViewDto>> GetAll(GetAllTaxSubCategoryInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredTaxSubCategory = _taxSubCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredTaxSubCategory = filteredTaxSubCategory
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var taxSubCategory = from o in pagedAndFilteredTaxSubCategory
                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         o.Code,
                                         o.IsActive,
                                         Id = o.Id
                                     };

                var totalCount = await filteredTaxSubCategory.CountAsync();

                var dbList = await taxSubCategory.ToListAsync();
                var results = new List<GetTaxSubCategoryForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetTaxSubCategoryForViewDto()
                    {
                        TaxSubCategory = new TaxSubCategoryDto
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

                return new PagedResultDto<GetTaxSubCategoryForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetTaxSubCategoryForViewDto> GetTaxSubCategoryForView(int id)
        {
            var taxSubCategory = await _taxSubCategoryRepository.GetAsync(id);

            var output = new GetTaxSubCategoryForViewDto { TaxSubCategory = ObjectMapper.Map<TaxSubCategoryDto>(taxSubCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaxSubCategory_Edit)]
        public async Task<GetTaxSubCategoryForEditOutput> GetTaxSubCategoryForEdit(EntityDto input)
        {
            var taxSubCategory = await _taxSubCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaxSubCategoryForEditOutput { TaxSubCategory = ObjectMapper.Map<CreateOrEditTaxSubCategoryDto>(taxSubCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaxSubCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaxSubCategory_Create)]
        protected virtual async Task Create(CreateOrEditTaxSubCategoryDto input)
        {
            var taxSubCategory = ObjectMapper.Map<TaxSubCategory>(input);
            taxSubCategory.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                taxSubCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _taxSubCategoryRepository.InsertAsync(taxSubCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaxSubCategory_Edit)]
        protected virtual async Task Update(CreateOrEditTaxSubCategoryDto input)
        {
            var taxSubCategory = await _taxSubCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taxSubCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaxSubCategory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _taxSubCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaxSubCategoryToExcel(GetAllTaxSubCategoryForExcelInput input)
        {

            var filteredTaxSubCategory = _taxSubCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredTaxSubCategory
                         select new GetTaxSubCategoryForViewDto()
                         {
                             TaxSubCategory = new TaxSubCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var taxSubCategoryListDtos = await query.ToListAsync();

            return _taxSubCategoryExcelExporter.ExportToFile(taxSubCategoryListDtos);
        }

    }
}