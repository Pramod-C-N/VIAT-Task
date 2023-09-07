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
    [AbpAuthorize(AppPermissions.Pages_IRNMasters)]
    public class IRNMastersAppService : vitaAppServiceBase, IIRNMastersAppService
    {
        private readonly IRepository<IRNMaster, long> _irnMasterRepository;

        public IRNMastersAppService(IRepository<IRNMaster, long> irnMasterRepository)
        {
            _irnMasterRepository = irnMasterRepository;

        }

        public async Task<PagedResultDto<GetIRNMasterForViewDto>> GetAll(GetAllIRNMastersInput input)
        {

            var filteredIRNMasters = _irnMasterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionType.Contains(input.Filter) || e.Status.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionTypeFilter), e => e.TransactionType.Contains(input.TransactionTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status.Contains(input.StatusFilter));

            var pagedAndFilteredIRNMasters = filteredIRNMasters
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var irnMasters = from o in pagedAndFilteredIRNMasters
                             select new
                             {

                                 o.TransactionType,
                                 o.Status,
                                 Id = o.Id
                             };

            var totalCount = await filteredIRNMasters.CountAsync();

            var dbList = await irnMasters.ToListAsync();
            var results = new List<GetIRNMasterForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetIRNMasterForViewDto()
                {
                    IRNMaster = new IRNMasterDto
                    {

                        TransactionType = o.TransactionType,
                        Status = o.Status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetIRNMasterForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetIRNMasterForViewDto> GetIRNMasterForView(long id)
        {
            var irnMaster = await _irnMasterRepository.GetAsync(id);

            var output = new GetIRNMasterForViewDto { IRNMaster = ObjectMapper.Map<IRNMasterDto>(irnMaster) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_IRNMasters_Edit)]
        public async Task<GetIRNMasterForEditOutput> GetIRNMasterForEdit(EntityDto<long> input)
        {
            var irnMaster = await _irnMasterRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetIRNMasterForEditOutput { IRNMaster = ObjectMapper.Map<CreateOrEditIRNMasterDto>(irnMaster) };

            return output;
        }

        public async Task<TransactionDto> CreateOrEdit(CreateOrEditIRNMasterDto input)
        {
               return await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_IRNMasters_Create)]
        protected virtual async Task<TransactionDto> Create(CreateOrEditIRNMasterDto input)
        {
            long id = 0;
            var irnMaster = ObjectMapper.Map<IRNMaster>(input);
            irnMaster.UniqueIdentifier = Guid.NewGuid();
            if (_irnMasterRepository.GetAll().Any())
                irnMaster.IRNNo = _irnMasterRepository.GetAll().Max(a => a.IRNNo) + 1;
            else
                irnMaster.IRNNo = 1;
            irnMaster.Status = "Processing";
            if (AbpSession.TenantId != null)
            {
                irnMaster.TenantId = (int?)AbpSession.TenantId;
            }
            try
            {
                id = await _irnMasterRepository.InsertAndGetIdAsync(irnMaster);
            }

            catch (Exception ex)
            {
                id = 1;
            }
            return new TransactionDto()
            {
                IRNNo = irnMaster.IRNNo,
                UniqueIdentifier = irnMaster.UniqueIdentifier
            };

        }

        [AbpAuthorize(AppPermissions.Pages_IRNMasters_Edit)]
        protected virtual async Task Update(CreateOrEditIRNMasterDto input)
        {
            var irnMaster = await _irnMasterRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, irnMaster);

        }

        [AbpAuthorize(AppPermissions.Pages_IRNMasters_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _irnMasterRepository.DeleteAsync(input.Id);
        }

    }
}