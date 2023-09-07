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
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteVATDetail)]
    public class PurchaseDebitNoteVATDetailAppService : vitaAppServiceBase, IPurchaseDebitNoteVATDetailAppService
    {
        private readonly IRepository<PurchaseDebitNoteVATDetail, long> _purchaseDebitNoteVATDetailRepository;

        public PurchaseDebitNoteVATDetailAppService(IRepository<PurchaseDebitNoteVATDetail, long> purchaseDebitNoteVATDetailRepository)
        {
            _purchaseDebitNoteVATDetailRepository = purchaseDebitNoteVATDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNoteVATDetailForViewDto>> GetAll(GetAllPurchaseDebitNoteVATDetailInput input)
        {

            var filteredPurchaseDebitNoteVATDetail = _purchaseDebitNoteVATDetailRepository.GetAll()
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

            var pagedAndFilteredPurchaseDebitNoteVATDetail = filteredPurchaseDebitNoteVATDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteVATDetail = from o in pagedAndFilteredPurchaseDebitNoteVATDetail
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

            var totalCount = await filteredPurchaseDebitNoteVATDetail.CountAsync();

            var dbList = await purchaseDebitNoteVATDetail.ToListAsync();
            var results = new List<GetPurchaseDebitNoteVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNoteVATDetailForViewDto()
                {
                    PurchaseDebitNoteVATDetail = new PurchaseDebitNoteVATDetailDto
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

            return new PagedResultDto<GetPurchaseDebitNoteVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Edit)]
        public async Task<GetPurchaseDebitNoteVATDetailForEditOutput> GetPurchaseDebitNoteVATDetailForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteVATDetail = await _purchaseDebitNoteVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNoteVATDetailForEditOutput { PurchaseDebitNoteVATDetail = ObjectMapper.Map<CreateOrEditPurchaseDebitNoteVATDetailDto>(purchaseDebitNoteVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNoteVATDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNoteVATDetailDto input)
        {
            var purchaseDebitNoteVATDetail = ObjectMapper.Map<PurchaseDebitNoteVATDetail>(input);
            purchaseDebitNoteVATDetail.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNoteVATDetailRepository.InsertAsync(purchaseDebitNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNoteVATDetailDto input)
        {
            var purchaseDebitNoteVATDetail = await _purchaseDebitNoteVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNoteVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}