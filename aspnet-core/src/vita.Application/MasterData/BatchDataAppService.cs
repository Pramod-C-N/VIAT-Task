using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(AppPermissions.Pages_BatchData)]
    public class BatchDataAppService : vitaAppServiceBase, IBatchDataAppService
    {
        private readonly IRepository<BatchData, long> _batchDataRepository;

        public BatchDataAppService(IRepository<BatchData, long> batchDataRepository)
        {
            _batchDataRepository = batchDataRepository;

        }

        public async Task<PagedResultDto<GetBatchDataForViewDto>> GetAll(GetAllBatchDataInput input)
        {

            var filteredBatchData = _batchDataRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.FilePath.Contains(input.Filter) || e.DataPath.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(input.MinfromDateFilter != null, e => e.fromDate >= input.MinfromDateFilter)
                        .WhereIf(input.MaxfromDateFilter != null, e => e.fromDate <= input.MaxfromDateFilter)
                        .WhereIf(input.MintoDateFilter != null, e => e.toDate >= input.MintoDateFilter)
                        .WhereIf(input.MaxtoDateFilter != null, e => e.toDate <= input.MaxtoDateFilter);

            var pagedAndFilteredBatchData = filteredBatchData
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var batchData = from o in pagedAndFilteredBatchData
                            select new
                            {

                                o.fromDate,
                                o.toDate,
                                Id = o.Id
                            };

            var totalCount = await filteredBatchData.CountAsync();

            var dbList = await batchData.ToListAsync();
            var results = new List<GetBatchDataForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBatchDataForViewDto()
                {
                    BatchData = new BatchDataDto
                    {

                        fromDate = o.fromDate,
                        toDate = o.toDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBatchDataForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_BatchData_Edit)]
        public async Task<GetBatchDataForEditOutput> GetBatchDataForEdit(EntityDto<long> input)
        {
            var batchData = await _batchDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBatchDataForEditOutput { BatchData = ObjectMapper.Map<CreateOrEditBatchDataDto>(batchData) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBatchDataDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BatchData_Create)]
        protected virtual async Task Create(CreateOrEditBatchDataDto input)
        {
            var batchData = ObjectMapper.Map<BatchData>(input);

            if (AbpSession.TenantId != null)
            {
                batchData.TenantId = (int?)AbpSession.TenantId;
            }

            await _batchDataRepository.InsertAsync(batchData);

        }

        [AbpAuthorize(AppPermissions.Pages_BatchData_Edit)]
        protected virtual async Task Update(CreateOrEditBatchDataDto input)
        {
            var batchData = await _batchDataRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, batchData);

        }

        [AbpAuthorize(AppPermissions.Pages_BatchData_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _batchDataRepository.DeleteAsync(input.Id);
        }

    }
}