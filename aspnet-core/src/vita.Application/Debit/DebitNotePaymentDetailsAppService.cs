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
    [AbpAuthorize(AppPermissions.Pages_DebitNotePaymentDetails)]
    public class DebitNotePaymentDetailsAppService : vitaAppServiceBase, IDebitNotePaymentDetailsAppService
    {
        private readonly IRepository<DebitNotePaymentDetail, long> _debitNotePaymentDetailRepository;

        public DebitNotePaymentDetailsAppService(IRepository<DebitNotePaymentDetail, long> debitNotePaymentDetailRepository)
        {
            _debitNotePaymentDetailRepository = debitNotePaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetDebitNotePaymentDetailForViewDto>> GetAll(GetAllDebitNotePaymentDetailsInput input)
        {

            var filteredDebitNotePaymentDetails = _debitNotePaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredDebitNotePaymentDetails = filteredDebitNotePaymentDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNotePaymentDetails = from o in pagedAndFilteredDebitNotePaymentDetails
                                          select new
                                          {

                                              o.IRNNo,
                                              o.PaymentMeans,
                                              o.CreditDebitReasonText,
                                              o.PaymentTerms,
                                              Id = o.Id
                                          };

            var totalCount = await filteredDebitNotePaymentDetails.CountAsync();

            var dbList = await debitNotePaymentDetails.ToListAsync();
            var results = new List<GetDebitNotePaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNotePaymentDetailForViewDto()
                {
                    DebitNotePaymentDetail = new DebitNotePaymentDetailDto
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

            return new PagedResultDto<GetDebitNotePaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotePaymentDetails_Edit)]
        public async Task<GetDebitNotePaymentDetailForEditOutput> GetDebitNotePaymentDetailForEdit(EntityDto<long> input)
        {
            var debitNotePaymentDetail = await _debitNotePaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNotePaymentDetailForEditOutput { DebitNotePaymentDetail = ObjectMapper.Map<CreateOrEditDebitNotePaymentDetailDto>(debitNotePaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNotePaymentDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotePaymentDetails_Create)]
        protected virtual async Task Create(CreateOrEditDebitNotePaymentDetailDto input)
        {
            var debitNotePaymentDetail = ObjectMapper.Map<DebitNotePaymentDetail>(input);
            debitNotePaymentDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNotePaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNotePaymentDetailRepository.InsertAsync(debitNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotePaymentDetails_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNotePaymentDetailDto input)
        {
            var debitNotePaymentDetail = await _debitNotePaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotePaymentDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNotePaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}