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
using IdentityServer4.Models;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftVATDetails)]
    public class DraftVATDetailsAppService : vitaAppServiceBase, IDraftVATDetailsAppService
    {
        private readonly IRepository<DraftVATDetail, long> _draftVATDetailRepository;

        public DraftVATDetailsAppService(IRepository<DraftVATDetail, long> draftVATDetailRepository)
        {
            _draftVATDetailRepository = draftVATDetailRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftVATDetailForViewDto>> GetAll(GetAllDraftVATDetailsInput input)
        {

            var filteredDraftVATDetails = _draftVATDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.ExcemptionReasonCode.Contains(input.Filter) || e.ExcemptionReasonText.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.Language.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter));

            var pagedAndFilteredDraftVATDetails = filteredDraftVATDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftVATDetails = from o in pagedAndFilteredDraftVATDetails
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
                                      o.AdditionalData1,
                                      o.Language,
                                      Id = o.Id
                                  };

            var totalCount = await filteredDraftVATDetails.CountAsync();

            var dbList = await draftVATDetails.ToListAsync();
            var results = new List<GetDraftVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftVATDetailForViewDto()
                {
                    DraftVATDetail = new DraftVATDetailDto
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
                        AdditionalData1 = o.AdditionalData1,
                        Language = o.Language,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DraftVATDetails_Edit)]
        public virtual async Task<GetDraftVATDetailForEditOutput> GetDraftVATDetailForEdit(EntityDto<long> input)
        {
            var draftVATDetail = await _draftVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftVATDetailForEditOutput { DraftVATDetail = ObjectMapper.Map<CreateOrEditDraftVATDetailDto>(draftVATDetail) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftVATDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftVATDetails_Create)]
        protected virtual async Task Create(CreateOrEditDraftVATDetailDto input)
        {
            var draftVATDetail = ObjectMapper.Map<DraftVATDetail>(input);
            draftVATDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                draftVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftVATDetailRepository.InsertAsync(draftVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftVATDetails_Edit)]
        protected virtual async Task Update(CreateOrEditDraftVATDetailDto input)
        {
            var draftVATDetail = await _draftVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            draftVATDetail.UniqueIdentifier = Guid.NewGuid();

            ObjectMapper.Map(input, draftVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftVATDetails_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}