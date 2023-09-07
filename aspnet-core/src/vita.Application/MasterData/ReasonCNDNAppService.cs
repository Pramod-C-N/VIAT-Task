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
    [AbpAuthorize(AppPermissions.Pages_ReasonCNDN)]
    public class ReasonCNDNAppService : vitaAppServiceBase, IReasonCNDNAppService
    {
        private readonly IRepository<ReasonCNDN> _reasonCNDNRepository;
        private readonly IReasonCNDNExcelExporter _reasonCNDNExcelExporter;

        public ReasonCNDNAppService(IRepository<ReasonCNDN> reasonCNDNRepository, IReasonCNDNExcelExporter reasonCNDNExcelExporter)
        {
            _reasonCNDNRepository = reasonCNDNRepository;
            _reasonCNDNExcelExporter = reasonCNDNExcelExporter;

        }

        public async Task<PagedResultDto<GetReasonCNDNForViewDto>> GetAll(GetAllReasonCNDNInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredReasonCNDN = _reasonCNDNRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredReasonCNDN = filteredReasonCNDN
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var reasonCNDN = from o in pagedAndFilteredReasonCNDN
                                 select new
                                 {

                                     o.Name,
                                     o.Description,
                                     o.Code,
                                     o.IsActive,
                                     Id = o.Id
                                 };

                var totalCount = await filteredReasonCNDN.CountAsync();

                var dbList = await reasonCNDN.ToListAsync();
                var results = new List<GetReasonCNDNForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetReasonCNDNForViewDto()
                    {
                        ReasonCNDN = new ReasonCNDNDto
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

                return new PagedResultDto<GetReasonCNDNForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        public async Task<GetReasonCNDNForViewDto> GetReasonCNDNForView(int id)
        {
            var reasonCNDN = await _reasonCNDNRepository.GetAsync(id);

            var output = new GetReasonCNDNForViewDto { ReasonCNDN = ObjectMapper.Map<ReasonCNDNDto>(reasonCNDN) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ReasonCNDN_Edit)]
        public async Task<GetReasonCNDNForEditOutput> GetReasonCNDNForEdit(EntityDto input)
        {
            var reasonCNDN = await _reasonCNDNRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReasonCNDNForEditOutput { ReasonCNDN = ObjectMapper.Map<CreateOrEditReasonCNDNDto>(reasonCNDN) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReasonCNDNDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ReasonCNDN_Create)]
        protected virtual async Task Create(CreateOrEditReasonCNDNDto input)
        {
            var reasonCNDN = ObjectMapper.Map<ReasonCNDN>(input);
            reasonCNDN.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                reasonCNDN.TenantId = (int?)AbpSession.TenantId;
            }

            await _reasonCNDNRepository.InsertAsync(reasonCNDN);

        }

        [AbpAuthorize(AppPermissions.Pages_ReasonCNDN_Edit)]
        protected virtual async Task Update(CreateOrEditReasonCNDNDto input)
        {
            var reasonCNDN = await _reasonCNDNRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, reasonCNDN);

        }

        [AbpAuthorize(AppPermissions.Pages_ReasonCNDN_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _reasonCNDNRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetReasonCNDNToExcel(GetAllReasonCNDNForExcelInput input)
        {

            var filteredReasonCNDN = _reasonCNDNRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredReasonCNDN
                         select new GetReasonCNDNForViewDto()
                         {
                             ReasonCNDN = new ReasonCNDNDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var reasonCNDNListDtos = await query.ToListAsync();

            return _reasonCNDNExcelExporter.ExportToFile(reasonCNDNListDtos);
        }

    }
}