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
    [AbpAuthorize(AppPermissions.Pages_HeadOfPayment)]
    public class HeadOfPaymentAppService : vitaAppServiceBase, IHeadOfPaymentAppService
    {
        private readonly IRepository<HeadOfPayment> _headOfPaymentRepository;
        private readonly IHeadOfPaymentExcelExporter _headOfPaymentExcelExporter;

        public HeadOfPaymentAppService(IRepository<HeadOfPayment> headOfPaymentRepository, IHeadOfPaymentExcelExporter headOfPaymentExcelExporter)
        {
            _headOfPaymentRepository = headOfPaymentRepository;
            _headOfPaymentExcelExporter = headOfPaymentExcelExporter;

        }

        public async Task<PagedResultDto<GetHeadOfPaymentForViewDto>> GetAll(GetAllHeadOfPaymentInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredHeadOfPayment = _headOfPaymentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.NatureOfService.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NatureOfServiceFilter), e => e.NatureOfService.Contains(input.NatureOfServiceFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredHeadOfPayment = filteredHeadOfPayment
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var headOfPayment = from o in pagedAndFilteredHeadOfPayment
                                    select new
                                    {

                                        o.Name,
                                        o.Description,
                                        o.Code,
                                        o.NatureOfService,
                                        o.IsActive,
                                        Id = o.Id
                                    };

                var totalCount = await filteredHeadOfPayment.CountAsync();

                var dbList = await headOfPayment.ToListAsync();
                var results = new List<GetHeadOfPaymentForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetHeadOfPaymentForViewDto()
                    {
                        HeadOfPayment = new HeadOfPaymentDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            NatureOfService = o.NatureOfService,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetHeadOfPaymentForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetHeadOfPaymentForViewDto> GetHeadOfPaymentForView(int id)
        {
            var headOfPayment = await _headOfPaymentRepository.GetAsync(id);

            var output = new GetHeadOfPaymentForViewDto { HeadOfPayment = ObjectMapper.Map<HeadOfPaymentDto>(headOfPayment) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HeadOfPayment_Edit)]
        public async Task<GetHeadOfPaymentForEditOutput> GetHeadOfPaymentForEdit(EntityDto input)
        {
            var headOfPayment = await _headOfPaymentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHeadOfPaymentForEditOutput { HeadOfPayment = ObjectMapper.Map<CreateOrEditHeadOfPaymentDto>(headOfPayment) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHeadOfPaymentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HeadOfPayment_Create)]
        protected virtual async Task Create(CreateOrEditHeadOfPaymentDto input)
        {
            var headOfPayment = ObjectMapper.Map<HeadOfPayment>(input);
            headOfPayment.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                headOfPayment.TenantId = (int?)AbpSession.TenantId;
            }

            await _headOfPaymentRepository.InsertAsync(headOfPayment);

        }

        [AbpAuthorize(AppPermissions.Pages_HeadOfPayment_Edit)]
        protected virtual async Task Update(CreateOrEditHeadOfPaymentDto input)
        {
            var headOfPayment = await _headOfPaymentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, headOfPayment);

        }

        [AbpAuthorize(AppPermissions.Pages_HeadOfPayment_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _headOfPaymentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHeadOfPaymentToExcel(GetAllHeadOfPaymentForExcelInput input)
        {

            var filteredHeadOfPayment = _headOfPaymentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.NatureOfService.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NatureOfServiceFilter), e => e.NatureOfService.Contains(input.NatureOfServiceFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredHeadOfPayment
                         select new GetHeadOfPaymentForViewDto()
                         {
                             HeadOfPayment = new HeadOfPaymentDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 NatureOfService = o.NatureOfService,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var headOfPaymentListDtos = await query.ToListAsync();

            return _headOfPaymentExcelExporter.ExportToFile(headOfPaymentListDtos);
        }

    }
}