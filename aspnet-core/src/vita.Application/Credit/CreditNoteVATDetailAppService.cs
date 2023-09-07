using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Credit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Credit
{
    [AbpAuthorize(AppPermissions.Pages_CreditNoteVATDetail)]
    public class CreditNoteVATDetailAppService : vitaAppServiceBase, ICreditNoteVATDetailAppService
    {
        private readonly IRepository<CreditNoteVATDetail, long> _creditNoteVATDetailRepository;

        public CreditNoteVATDetailAppService(IRepository<CreditNoteVATDetail, long> creditNoteVATDetailRepository)
        {
            _creditNoteVATDetailRepository = creditNoteVATDetailRepository;

        }

        public async Task<PagedResultDto<GetCreditNoteVATDetailForViewDto>> GetAll(GetAllCreditNoteVATDetailInput input)
        {

            var filteredCreditNoteVATDetail = _creditNoteVATDetailRepository.GetAll()
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

            var pagedAndFilteredCreditNoteVATDetail = filteredCreditNoteVATDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteVATDetail = from o in pagedAndFilteredCreditNoteVATDetail
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

            var totalCount = await filteredCreditNoteVATDetail.CountAsync();

            var dbList = await creditNoteVATDetail.ToListAsync();
            var results = new List<GetCreditNoteVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteVATDetailForViewDto()
                {
                    CreditNoteVATDetail = new CreditNoteVATDetailDto
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

            return new PagedResultDto<GetCreditNoteVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteVATDetail_Edit)]
        public async Task<GetCreditNoteVATDetailForEditOutput> GetCreditNoteVATDetailForEdit(EntityDto<long> input)
        {
            var creditNoteVATDetail = await _creditNoteVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteVATDetailForEditOutput { CreditNoteVATDetail = ObjectMapper.Map<CreateOrEditCreditNoteVATDetailDto>(creditNoteVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNoteVATDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteVATDetail_Create)]
        protected virtual async Task Create(CreateOrEditCreditNoteVATDetailDto input)
        {
            var creditNoteVATDetail = ObjectMapper.Map<CreditNoteVATDetail>(input);
            creditNoteVATDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNoteVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNoteVATDetailRepository.InsertAsync(creditNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteVATDetail_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteVATDetailDto input)
        {
            var creditNoteVATDetail = await _creditNoteVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteVATDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNoteVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}