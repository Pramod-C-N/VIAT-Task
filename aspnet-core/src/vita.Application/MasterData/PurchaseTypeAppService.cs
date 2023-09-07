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
    [AbpAuthorize(AppPermissions.Pages_PurchaseType)]
    public class PurchaseTypeAppService : vitaAppServiceBase, IPurchaseTypeAppService
    {
        private readonly IRepository<PurchaseType> _purchaseTypeRepository;
        private readonly IPurchaseTypeExcelExporter _purchaseTypeExcelExporter;

        public PurchaseTypeAppService(IRepository<PurchaseType> purchaseTypeRepository, IPurchaseTypeExcelExporter purchaseTypeExcelExporter)
        {
            _purchaseTypeRepository = purchaseTypeRepository;
            _purchaseTypeExcelExporter = purchaseTypeExcelExporter;

        }

        public async Task<PagedResultDto<GetPurchaseTypeForViewDto>> GetAll(GetAllPurchaseTypeInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredPurchaseType = _purchaseTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredPurchaseType = filteredPurchaseType
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var purchaseType = from o in pagedAndFilteredPurchaseType
                                   select new
                                   {

                                       o.Name,
                                       o.Description,
                                       o.Code,
                                       o.IsActive,
                                       Id = o.Id
                                   };

                var totalCount = await filteredPurchaseType.CountAsync();

                var dbList = await purchaseType.ToListAsync();
                var results = new List<GetPurchaseTypeForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetPurchaseTypeForViewDto()
                    {
                        PurchaseType = new PurchaseTypeDto
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

                return new PagedResultDto<GetPurchaseTypeForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public async Task<GetPurchaseTypeForViewDto> GetPurchaseTypeForView(int id)
        {
            var purchaseType = await _purchaseTypeRepository.GetAsync(id);

            var output = new GetPurchaseTypeForViewDto { PurchaseType = ObjectMapper.Map<PurchaseTypeDto>(purchaseType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseType_Edit)]
        public async Task<GetPurchaseTypeForEditOutput> GetPurchaseTypeForEdit(EntityDto input)
        {
            var purchaseType = await _purchaseTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseTypeForEditOutput { PurchaseType = ObjectMapper.Map<CreateOrEditPurchaseTypeDto>(purchaseType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseType_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseTypeDto input)
        {
            var purchaseType = ObjectMapper.Map<PurchaseType>(input);
            purchaseType.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseType.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseTypeRepository.InsertAsync(purchaseType);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseType_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseTypeDto input)
        {
            var purchaseType = await _purchaseTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, purchaseType);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _purchaseTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPurchaseTypeToExcel(GetAllPurchaseTypeForExcelInput input)
        {

            var filteredPurchaseType = _purchaseTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredPurchaseType
                         select new GetPurchaseTypeForViewDto()
                         {
                             PurchaseType = new PurchaseTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var purchaseTypeListDtos = await query.ToListAsync();

            return _purchaseTypeExcelExporter.ExportToFile(purchaseTypeListDtos);
        }

    }
}