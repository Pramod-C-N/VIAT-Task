using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Debit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Debit
{
    [AbpAuthorize(AppPermissions.Pages_DebitNoteDiscounts)]
    public class DebitNoteDiscountsAppService : vitaAppServiceBase, IDebitNoteDiscountsAppService
    {
        private readonly IRepository<DebitNoteDiscount, long> _debitNoteDiscountRepository;

        public DebitNoteDiscountsAppService(IRepository<DebitNoteDiscount, long> debitNoteDiscountRepository)
        {
            _debitNoteDiscountRepository = debitNoteDiscountRepository;

        }

        public async Task<PagedResultDto<GetDebitNoteDiscountForViewDto>> GetAll(GetAllDebitNoteDiscountsInput input)
        {

            var filteredDebitNoteDiscounts = _debitNoteDiscountRepository.GetAll()
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

            var pagedAndFilteredDebitNoteDiscounts = filteredDebitNoteDiscounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteDiscounts = from o in pagedAndFilteredDebitNoteDiscounts
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

            var totalCount = await filteredDebitNoteDiscounts.CountAsync();

            var dbList = await debitNoteDiscounts.ToListAsync();
            var results = new List<GetDebitNoteDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteDiscountForViewDto()
                {
                    DebitNoteDiscount = new DebitNoteDiscountDto
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

            return new PagedResultDto<GetDebitNoteDiscountForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteDiscounts_Edit)]
        public async Task<GetDebitNoteDiscountForEditOutput> GetDebitNoteDiscountForEdit(EntityDto<long> input)
        {
            var debitNoteDiscount = await _debitNoteDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteDiscountForEditOutput { DebitNoteDiscount = ObjectMapper.Map<CreateOrEditDebitNoteDiscountDto>(debitNoteDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteDiscountDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteDiscounts_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteDiscountDto input)
        {
            var debitNoteDiscount = ObjectMapper.Map<DebitNoteDiscount>(input);
            debitNoteDiscount.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNoteDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteDiscountRepository.InsertAsync(debitNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteDiscounts_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteDiscountDto input)
        {
            var debitNoteDiscount = await _debitNoteDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteDiscounts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteDiscountRepository.DeleteAsync(input.Id);
        }

    }
}