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
    [AbpAuthorize(AppPermissions.Pages_DraftPaymentDetails)]
    public class DraftPaymentDetailsAppService : vitaAppServiceBase, IDraftPaymentDetailsAppService
    {
        private readonly IRepository<DraftPaymentDetail, long> _draftPaymentDetailRepository;

        public DraftPaymentDetailsAppService(IRepository<DraftPaymentDetail, long> draftPaymentDetailRepository)
        {
            _draftPaymentDetailRepository = draftPaymentDetailRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftPaymentDetailForViewDto>> GetAll(GetAllDraftPaymentDetailsInput input)
        {

            var filteredDraftPaymentDetails = _draftPaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.Language.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter));

            var pagedAndFilteredDraftPaymentDetails = filteredDraftPaymentDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftPaymentDetails = from o in pagedAndFilteredDraftPaymentDetails
                                      select new
                                      {

                                          o.IRNNo,
                                          o.PaymentMeans,
                                          o.CreditDebitReasonText,
                                          o.PaymentTerms,
                                          o.AdditionalData1,
                                          o.Language,
                                          Id = o.Id
                                      };

            var totalCount = await filteredDraftPaymentDetails.CountAsync();

            var dbList = await draftPaymentDetails.ToListAsync();
            var results = new List<GetDraftPaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftPaymentDetailForViewDto()
                {
                    DraftPaymentDetail = new DraftPaymentDetailDto
                    {

                        IRNNo = o.IRNNo,
                        PaymentMeans = o.PaymentMeans,
                        CreditDebitReasonText = o.CreditDebitReasonText,
                        PaymentTerms = o.PaymentTerms,
                        AdditionalData1 = o.AdditionalData1,
                        Language = o.Language,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftPaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftPaymentDetailForViewDto> GetDraftPaymentDetailForView(long id)
        {
            var draftPaymentDetail = await _draftPaymentDetailRepository.GetAsync(id);

            var output = new GetDraftPaymentDetailForViewDto { DraftPaymentDetail = ObjectMapper.Map<DraftPaymentDetailDto>(draftPaymentDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftPaymentDetails_Edit)]
        public virtual async Task<GetDraftPaymentDetailForEditOutput> GetDraftPaymentDetailForEdit(EntityDto<long> input)
        {
            var draftPaymentDetail = await _draftPaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftPaymentDetailForEditOutput { DraftPaymentDetail = ObjectMapper.Map<CreateOrEditDraftPaymentDetailDto>(draftPaymentDetail) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftPaymentDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftPaymentDetails_Create)]
        protected virtual async Task Create(CreateOrEditDraftPaymentDetailDto input)
        {
            var draftPaymentDetail = ObjectMapper.Map<DraftPaymentDetail>(input);
            draftPaymentDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                draftPaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftPaymentDetailRepository.InsertAsync(draftPaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftPaymentDetails_Edit)]
        protected virtual async Task Update(CreateOrEditDraftPaymentDetailDto input)
        {
            var draftPaymentDetail = await _draftPaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            draftPaymentDetail.UniqueIdentifier = Guid.NewGuid();

            ObjectMapper.Map(input, draftPaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftPaymentDetails_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftPaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}