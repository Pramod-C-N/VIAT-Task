using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseDebit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseDebit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteDiscount)]
    public class PurchaseDebitNoteDiscountAppService : vitaAppServiceBase, IPurchaseDebitNoteDiscountAppService
    {
        private readonly IRepository<PurchaseDebitNoteDiscount, long> _purchaseDebitNoteDiscountRepository;

        public PurchaseDebitNoteDiscountAppService(IRepository<PurchaseDebitNoteDiscount, long> purchaseDebitNoteDiscountRepository)
        {
            _purchaseDebitNoteDiscountRepository = purchaseDebitNoteDiscountRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNoteDiscountForViewDto>> GetAll(GetAllPurchaseDebitNoteDiscountInput input)
        {

            var filteredPurchaseDebitNoteDiscount = _purchaseDebitNoteDiscountRepository.GetAll()
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

            var pagedAndFilteredPurchaseDebitNoteDiscount = filteredPurchaseDebitNoteDiscount
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteDiscount = from o in pagedAndFilteredPurchaseDebitNoteDiscount
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

            var totalCount = await filteredPurchaseDebitNoteDiscount.CountAsync();

            var dbList = await purchaseDebitNoteDiscount.ToListAsync();
            var results = new List<GetPurchaseDebitNoteDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNoteDiscountForViewDto()
                {
                    PurchaseDebitNoteDiscount = new PurchaseDebitNoteDiscountDto
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

            return new PagedResultDto<GetPurchaseDebitNoteDiscountForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseDebitNoteDiscountForViewDto> GetPurchaseDebitNoteDiscountForView(long id)
        {
            var purchaseDebitNoteDiscount = await _purchaseDebitNoteDiscountRepository.GetAsync(id);

            var output = new GetPurchaseDebitNoteDiscountForViewDto { PurchaseDebitNoteDiscount = ObjectMapper.Map<PurchaseDebitNoteDiscountDto>(purchaseDebitNoteDiscount) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteDiscount_Edit)]
        public async Task<GetPurchaseDebitNoteDiscountForEditOutput> GetPurchaseDebitNoteDiscountForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteDiscount = await _purchaseDebitNoteDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNoteDiscountForEditOutput { PurchaseDebitNoteDiscount = ObjectMapper.Map<CreateOrEditPurchaseDebitNoteDiscountDto>(purchaseDebitNoteDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNoteDiscountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteDiscount_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNoteDiscountDto input)
        {
            var purchaseDebitNoteDiscount = ObjectMapper.Map<PurchaseDebitNoteDiscount>(input);

            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNoteDiscountRepository.InsertAsync(purchaseDebitNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteDiscount_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNoteDiscountDto input)
        {
            var purchaseDebitNoteDiscount = await _purchaseDebitNoteDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteDiscount_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNoteDiscountRepository.DeleteAsync(input.Id);
        }

    }
}