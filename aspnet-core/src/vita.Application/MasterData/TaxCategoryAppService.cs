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
    [AbpAuthorize(AppPermissions.Pages_TaxCategory)]
    public class TaxCategoryAppService : vitaAppServiceBase, ITaxCategoryAppService
    {
        private readonly IRepository<TaxCategory> _taxCategoryRepository;
        private readonly ITaxCategoryExcelExporter _taxCategoryExcelExporter;

        public TaxCategoryAppService(IRepository<TaxCategory> taxCategoryRepository, ITaxCategoryExcelExporter taxCategoryExcelExporter)
        {
            _taxCategoryRepository = taxCategoryRepository;
            _taxCategoryExcelExporter = taxCategoryExcelExporter;

        }

        public async Task<PagedResultDto<GetTaxCategoryForViewDto>> GetAll(GetAllTaxCategoryInput input)
        {

            var filteredTaxCategory = _taxCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.IsKSAApplicable.Contains(input.Filter) || e.TaxSchemeID.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsKSAApplicableFilter), e => e.IsKSAApplicable.Contains(input.IsKSAApplicableFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIDFilter), e => e.TaxSchemeID.Contains(input.TaxSchemeIDFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTaxCategory = filteredTaxCategory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taxCategory = from o in pagedAndFilteredTaxCategory
                              select new
                              {

                                  o.Name,
                                  o.Description,
                                  o.Code,
                                  o.IsKSAApplicable,
                                  o.TaxSchemeID,
                                  o.IsActive,
                                  Id = o.Id
                              };

            var totalCount = await filteredTaxCategory.CountAsync();

            var dbList = await taxCategory.ToListAsync();
            var results = new List<GetTaxCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaxCategoryForViewDto()
                {
                    TaxCategory = new TaxCategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Code = o.Code,
                        IsKSAApplicable = o.IsKSAApplicable,
                        TaxSchemeID = o.TaxSchemeID,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaxCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaxCategoryForViewDto> GetTaxCategoryForView(int id)
        {
            var taxCategory = await _taxCategoryRepository.GetAsync(id);

            var output = new GetTaxCategoryForViewDto { TaxCategory = ObjectMapper.Map<TaxCategoryDto>(taxCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaxCategory_Edit)]
        public async Task<GetTaxCategoryForEditOutput> GetTaxCategoryForEdit(EntityDto input)
        {
            var taxCategory = await _taxCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaxCategoryForEditOutput { TaxCategory = ObjectMapper.Map<CreateOrEditTaxCategoryDto>(taxCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaxCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaxCategory_Create)]
        protected virtual async Task Create(CreateOrEditTaxCategoryDto input)
        {
            var taxCategory = ObjectMapper.Map<TaxCategory>(input);
            taxCategory.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                taxCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _taxCategoryRepository.InsertAsync(taxCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaxCategory_Edit)]
        protected virtual async Task Update(CreateOrEditTaxCategoryDto input)
        {
            var taxCategory = await _taxCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taxCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaxCategory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _taxCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaxCategoryToExcel(GetAllTaxCategoryForExcelInput input)
        {

            var filteredTaxCategory = _taxCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.IsKSAApplicable.Contains(input.Filter) || e.TaxSchemeID.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsKSAApplicableFilter), e => e.IsKSAApplicable.Contains(input.IsKSAApplicableFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIDFilter), e => e.TaxSchemeID.Contains(input.TaxSchemeIDFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredTaxCategory
                         select new GetTaxCategoryForViewDto()
                         {
                             TaxCategory = new TaxCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsKSAApplicable = o.IsKSAApplicable,
                                 TaxSchemeID = o.TaxSchemeID,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var taxCategoryListDtos = await query.ToListAsync();

            return _taxCategoryExcelExporter.ExportToFile(taxCategoryListDtos);
        }

    }
}