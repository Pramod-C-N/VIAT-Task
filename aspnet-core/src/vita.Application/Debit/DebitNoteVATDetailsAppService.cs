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
    [AbpAuthorize(AppPermissions.Pages_DebitNoteVATDetails)]
    public class DebitNoteVATDetailsAppService : vitaAppServiceBase, IDebitNoteVATDetailsAppService
    {
        private readonly IRepository<DebitNoteVATDetail, long> _debitNoteVATDetailRepository;

        public DebitNoteVATDetailsAppService(IRepository<DebitNoteVATDetail, long> debitNoteVATDetailRepository)
        {
            _debitNoteVATDetailRepository = debitNoteVATDetailRepository;

        }

        public async Task<PagedResultDto<GetDebitNoteVATDetailForViewDto>> GetAll(GetAllDebitNoteVATDetailsInput input)
        {

            var filteredDebitNoteVATDetails = _debitNoteVATDetailRepository.GetAll()
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

            var pagedAndFilteredDebitNoteVATDetails = filteredDebitNoteVATDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteVATDetails = from o in pagedAndFilteredDebitNoteVATDetails
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

            var totalCount = await filteredDebitNoteVATDetails.CountAsync();

            var dbList = await debitNoteVATDetails.ToListAsync();
            var results = new List<GetDebitNoteVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteVATDetailForViewDto()
                {
                    DebitNoteVATDetail = new DebitNoteVATDetailDto
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

            return new PagedResultDto<GetDebitNoteVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteVATDetails_Edit)]
        public async Task<GetDebitNoteVATDetailForEditOutput> GetDebitNoteVATDetailForEdit(EntityDto<long> input)
        {
            var debitNoteVATDetail = await _debitNoteVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteVATDetailForEditOutput { DebitNoteVATDetail = ObjectMapper.Map<CreateOrEditDebitNoteVATDetailDto>(debitNoteVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteVATDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteVATDetails_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteVATDetailDto input)
        {
            var debitNoteVATDetail = ObjectMapper.Map<DebitNoteVATDetail>(input);
            debitNoteVATDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNoteVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteVATDetailRepository.InsertAsync(debitNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteVATDetails_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteVATDetailDto input)
        {
            var debitNoteVATDetail = await _debitNoteVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteVATDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}