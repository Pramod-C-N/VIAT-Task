using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.DraftFee.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftDiscounts)]
    public class DraftDiscountsAppService : vitaAppServiceBase, IDraftDiscountsAppService
    {
        private readonly IRepository<DraftDiscount, long> _draftDiscountRepository;

        public DraftDiscountsAppService(IRepository<DraftDiscount, long> draftDiscountRepository)
        {
            _draftDiscountRepository = draftDiscountRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftDiscountForViewDto>> GetAll(GetAllDraftDiscountsInput input)
        {

            var filteredDraftDiscounts = _draftDiscountRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.MinVATRateFilter != null, e => e.VATRate >= input.MinVATRateFilter)
                        .WhereIf(input.MaxVATRateFilter != null, e => e.VATRate <= input.MaxVATRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIdFilter), e => e.TaxSchemeId.Contains(input.TaxSchemeIdFilter));

            var pagedAndFilteredDraftDiscounts = filteredDraftDiscounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftDiscounts = from o in pagedAndFilteredDraftDiscounts
                                 select new
                                 {

                                     o.IRNNo,
                                     o.DiscountPercentage,
                                     o.DiscountAmount,
                                     o.VATCode,
                                     o.VATRate,
                                     o.TaxSchemeId,
                                     Id = o.Id
                                 };

            var totalCount = await filteredDraftDiscounts.CountAsync();

            var dbList = await draftDiscounts.ToListAsync();
            var results = new List<GetDraftDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftDiscountForViewDto()
                {
                    DraftDiscount = new DraftDiscountDto
                    {

                        IRNNo = o.IRNNo,
                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        VATCode = o.VATCode,
                        VATRate = o.VATRate,
                        TaxSchemeId = o.TaxSchemeId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftDiscountForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftDiscountForViewDto> GetDraftDiscountForView(long id)
        {
            var draftDiscount = await _draftDiscountRepository.GetAsync(id);

            var output = new GetDraftDiscountForViewDto { DraftDiscount = ObjectMapper.Map<DraftDiscountDto>(draftDiscount) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftDiscounts_Edit)]
        public virtual async Task<GetDraftDiscountForEditOutput> GetDraftDiscountForEdit(EntityDto<long> input)
        {
            var draftDiscount = await _draftDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftDiscountForEditOutput { DraftDiscount = ObjectMapper.Map<CreateOrEditDraftDiscountDto>(draftDiscount) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftDiscountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftDiscounts_Create)]
        protected virtual async Task Create(CreateOrEditDraftDiscountDto input)
        {
            var draftDiscount = ObjectMapper.Map<DraftDiscount>(input);

            if (AbpSession.TenantId != null)
            {
                draftDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftDiscountRepository.InsertAsync(draftDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftDiscounts_Edit)]
        protected virtual async Task Update(CreateOrEditDraftDiscountDto input)
        {
            var draftDiscount = await _draftDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, draftDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftDiscounts_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftDiscountRepository.DeleteAsync(input.Id);
        }

    }
}