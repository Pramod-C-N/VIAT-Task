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
    [AbpAuthorize(AppPermissions.Pages_CreditNotePaymentDetail)]
    public class CreditNotePaymentDetailAppService : vitaAppServiceBase, ICreditNotePaymentDetailAppService
    {
        private readonly IRepository<CreditNotePaymentDetail, long> _creditNotePaymentDetailRepository;

        public CreditNotePaymentDetailAppService(IRepository<CreditNotePaymentDetail, long> creditNotePaymentDetailRepository)
        {
            _creditNotePaymentDetailRepository = creditNotePaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetCreditNotePaymentDetailForViewDto>> GetAll(GetAllCreditNotePaymentDetailInput input)
        {

            var filteredCreditNotePaymentDetail = _creditNotePaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredCreditNotePaymentDetail = filteredCreditNotePaymentDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNotePaymentDetail = from o in pagedAndFilteredCreditNotePaymentDetail
                                          select new
                                          {

                                              o.IRNNo,
                                              o.PaymentMeans,
                                              o.CreditDebitReasonText,
                                              o.PaymentTerms,
                                              Id = o.Id
                                          };

            var totalCount = await filteredCreditNotePaymentDetail.CountAsync();

            var dbList = await creditNotePaymentDetail.ToListAsync();
            var results = new List<GetCreditNotePaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNotePaymentDetailForViewDto()
                {
                    CreditNotePaymentDetail = new CreditNotePaymentDetailDto
                    {

                        IRNNo = o.IRNNo,
                        PaymentMeans = o.PaymentMeans,
                        CreditDebitReasonText = o.CreditDebitReasonText,
                        PaymentTerms = o.PaymentTerms,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCreditNotePaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCreditNotePaymentDetailForViewDto> GetCreditNotePaymentDetailForView(long id)
        {
            var creditNotePaymentDetail = await _creditNotePaymentDetailRepository.GetAsync(id);

            var output = new GetCreditNotePaymentDetailForViewDto { CreditNotePaymentDetail = ObjectMapper.Map<CreditNotePaymentDetailDto>(creditNotePaymentDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNotePaymentDetail_Edit)]
        public async Task<GetCreditNotePaymentDetailForEditOutput> GetCreditNotePaymentDetailForEdit(EntityDto<long> input)
        {
            var creditNotePaymentDetail = await _creditNotePaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNotePaymentDetailForEditOutput { CreditNotePaymentDetail = ObjectMapper.Map<CreateOrEditCreditNotePaymentDetailDto>(creditNotePaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNotePaymentDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNotePaymentDetail_Create)]
        protected virtual async Task Create(CreateOrEditCreditNotePaymentDetailDto input)
        {
            var creditNotePaymentDetail = ObjectMapper.Map<CreditNotePaymentDetail>(input);
            creditNotePaymentDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNotePaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNotePaymentDetailRepository.InsertAsync(creditNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNotePaymentDetail_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNotePaymentDetailDto input)
        {
            var creditNotePaymentDetail = await _creditNotePaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNotePaymentDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNotePaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}