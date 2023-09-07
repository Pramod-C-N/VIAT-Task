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
    [AbpAuthorize(AppPermissions.Pages_CreditNoteDiscount)]
    public class CreditNoteDiscountAppService : vitaAppServiceBase, ICreditNoteDiscountAppService
    {
        private readonly IRepository<CreditNoteDiscount, long> _creditNoteDiscountRepository;

        public CreditNoteDiscountAppService(IRepository<CreditNoteDiscount, long> creditNoteDiscountRepository)
        {
            _creditNoteDiscountRepository = creditNoteDiscountRepository;

        }

        public async Task<PagedResultDto<GetCreditNoteDiscountForViewDto>> GetAll(GetAllCreditNoteDiscountInput input)
        {

            var filteredCreditNoteDiscount = _creditNoteDiscountRepository.GetAll()
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

            var pagedAndFilteredCreditNoteDiscount = filteredCreditNoteDiscount
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteDiscount = from o in pagedAndFilteredCreditNoteDiscount
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

            var totalCount = await filteredCreditNoteDiscount.CountAsync();

            var dbList = await creditNoteDiscount.ToListAsync();
            var results = new List<GetCreditNoteDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteDiscountForViewDto()
                {
                    CreditNoteDiscount = new CreditNoteDiscountDto
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

            return new PagedResultDto<GetCreditNoteDiscountForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteDiscount_Edit)]
        public async Task<GetCreditNoteDiscountForEditOutput> GetCreditNoteDiscountForEdit(EntityDto<long> input)
        {
            var creditNoteDiscount = await _creditNoteDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteDiscountForEditOutput { CreditNoteDiscount = ObjectMapper.Map<CreateOrEditCreditNoteDiscountDto>(creditNoteDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNoteDiscountDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteDiscount_Create)]
        protected virtual async Task Create(CreateOrEditCreditNoteDiscountDto input)
        {
            var creditNoteDiscount = ObjectMapper.Map<CreditNoteDiscount>(input);
            creditNoteDiscount.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNoteDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNoteDiscountRepository.InsertAsync(creditNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteDiscount_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteDiscountDto input)
        {
            var creditNoteDiscount = await _creditNoteDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteDiscount_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNoteDiscountRepository.DeleteAsync(input.Id);
        }

    }
}