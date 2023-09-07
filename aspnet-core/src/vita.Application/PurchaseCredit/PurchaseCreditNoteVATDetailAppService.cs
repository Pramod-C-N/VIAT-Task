using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseCredit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseCredit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteVATDetail)]
    public class PurchaseCreditNoteVATDetailAppService : vitaAppServiceBase, IPurchaseCreditNoteVATDetailAppService
    {
        private readonly IRepository<PurchaseCreditNoteVATDetail, long> _purchaseCreditNoteVATDetailRepository;

        public PurchaseCreditNoteVATDetailAppService(IRepository<PurchaseCreditNoteVATDetail, long> purchaseCreditNoteVATDetailRepository)
        {
            _purchaseCreditNoteVATDetailRepository = purchaseCreditNoteVATDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNoteVATDetailForViewDto>> GetAll(GetAllPurchaseCreditNoteVATDetailInput input)
        {

            var filteredPurchaseCreditNoteVATDetail = _purchaseCreditNoteVATDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.ExcemptionReasonCode.Contains(input.Filter) || e.ExcemptionReasonText.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIdFilter), e => e.TaxSchemeId.Contains(input.TaxSchemeIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.MinVATRateFilter != null, e => e.VATRate >= input.MinVATRateFilter)
                        .WhereIf(input.MaxVATRateFilter != null, e => e.VATRate <= input.MaxVATRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonCodeFilter), e => e.ExcemptionReasonCode.Contains(input.ExcemptionReasonCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonTextFilter), e => e.ExcemptionReasonText.Contains(input.ExcemptionReasonTextFilter))
                        .WhereIf(input.MinTaxableAmountFilter != null, e => e.TaxableAmount >= input.MinTaxableAmountFilter)
                        .WhereIf(input.MaxTaxableAmountFilter != null, e => e.TaxableAmount <= input.MaxTaxableAmountFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter));

            var pagedAndFilteredPurchaseCreditNoteVATDetail = filteredPurchaseCreditNoteVATDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNoteVATDetail = from o in pagedAndFilteredPurchaseCreditNoteVATDetail
                                              select new
                                              {

                                                  o.IRNNo,
                                                  o.TaxSchemeId,
                                                  o.VATCode,
                                                  o.VATRate,
                                                  o.ExcemptionReasonCode,
                                                  o.ExcemptionReasonText,
                                                  o.TaxableAmount,
                                                  o.TaxAmount,
                                                  o.CurrencyCode,
                                                  Id = o.Id
                                              };

            var totalCount = await filteredPurchaseCreditNoteVATDetail.CountAsync();

            var dbList = await purchaseCreditNoteVATDetail.ToListAsync();
            var results = new List<GetPurchaseCreditNoteVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNoteVATDetailForViewDto()
                {
                    PurchaseCreditNoteVATDetail = new PurchaseCreditNoteVATDetailDto
                    {

                        IRNNo = o.IRNNo,
                        TaxSchemeId = o.TaxSchemeId,
                        VATCode = o.VATCode,
                        VATRate = o.VATRate,
                        ExcemptionReasonCode = o.ExcemptionReasonCode,
                        ExcemptionReasonText = o.ExcemptionReasonText,
                        TaxableAmount = o.TaxableAmount,
                        TaxAmount = o.TaxAmount,
                        CurrencyCode = o.CurrencyCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseCreditNoteVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNoteVATDetailForViewDto> GetPurchaseCreditNoteVATDetailForView(long id)
        {
            var purchaseCreditNoteVATDetail = await _purchaseCreditNoteVATDetailRepository.GetAsync(id);

            var output = new GetPurchaseCreditNoteVATDetailForViewDto { PurchaseCreditNoteVATDetail = ObjectMapper.Map<PurchaseCreditNoteVATDetailDto>(purchaseCreditNoteVATDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Edit)]
        public async Task<GetPurchaseCreditNoteVATDetailForEditOutput> GetPurchaseCreditNoteVATDetailForEdit(EntityDto<long> input)
        {
            var purchaseCreditNoteVATDetail = await _purchaseCreditNoteVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNoteVATDetailForEditOutput { PurchaseCreditNoteVATDetail = ObjectMapper.Map<CreateOrEditPurchaseCreditNoteVATDetailDto>(purchaseCreditNoteVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNoteVATDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNoteVATDetailDto input)
        {
            var purchaseCreditNoteVATDetail = ObjectMapper.Map<PurchaseCreditNoteVATDetail>(input);
            purchaseCreditNoteVATDetail.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseCreditNoteVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNoteVATDetailRepository.InsertAsync(purchaseCreditNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNoteVATDetailDto input)
        {
            var purchaseCreditNoteVATDetail = await _purchaseCreditNoteVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNoteVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}