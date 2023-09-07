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
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNotePaymentDetail)]
    public class PurchaseCreditNotePaymentDetailAppService : vitaAppServiceBase, IPurchaseCreditNotePaymentDetailAppService
    {
        private readonly IRepository<PurchaseCreditNotePaymentDetail, long> _purchaseCreditNotePaymentDetailRepository;

        public PurchaseCreditNotePaymentDetailAppService(IRepository<PurchaseCreditNotePaymentDetail, long> purchaseCreditNotePaymentDetailRepository)
        {
            _purchaseCreditNotePaymentDetailRepository = purchaseCreditNotePaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNotePaymentDetailForViewDto>> GetAll(GetAllPurchaseCreditNotePaymentDetailInput input)
        {

            var filteredPurchaseCreditNotePaymentDetail = _purchaseCreditNotePaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredPurchaseCreditNotePaymentDetail = filteredPurchaseCreditNotePaymentDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNotePaymentDetail = from o in pagedAndFilteredPurchaseCreditNotePaymentDetail
                                                  select new
                                                  {

                                                      o.IRNNo,
                                                      o.PaymentMeans,
                                                      o.CreditDebitReasonText,
                                                      o.PaymentTerms,
                                                      Id = o.Id
                                                  };

            var totalCount = await filteredPurchaseCreditNotePaymentDetail.CountAsync();

            var dbList = await purchaseCreditNotePaymentDetail.ToListAsync();
            var results = new List<GetPurchaseCreditNotePaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNotePaymentDetailForViewDto()
                {
                    PurchaseCreditNotePaymentDetail = new PurchaseCreditNotePaymentDetailDto
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

            return new PagedResultDto<GetPurchaseCreditNotePaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNotePaymentDetailForViewDto> GetPurchaseCreditNotePaymentDetailForView(long id)
        {
            var purchaseCreditNotePaymentDetail = await _purchaseCreditNotePaymentDetailRepository.GetAsync(id);

            var output = new GetPurchaseCreditNotePaymentDetailForViewDto { PurchaseCreditNotePaymentDetail = ObjectMapper.Map<PurchaseCreditNotePaymentDetailDto>(purchaseCreditNotePaymentDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Edit)]
        public async Task<GetPurchaseCreditNotePaymentDetailForEditOutput> GetPurchaseCreditNotePaymentDetailForEdit(EntityDto<long> input)
        {
            var purchaseCreditNotePaymentDetail = await _purchaseCreditNotePaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNotePaymentDetailForEditOutput { PurchaseCreditNotePaymentDetail = ObjectMapper.Map<CreateOrEditPurchaseCreditNotePaymentDetailDto>(purchaseCreditNotePaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNotePaymentDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNotePaymentDetailDto input)
        {
            var purchaseCreditNotePaymentDetail = ObjectMapper.Map<PurchaseCreditNotePaymentDetail>(input);
            purchaseCreditNotePaymentDetail.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseCreditNotePaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNotePaymentDetailRepository.InsertAsync(purchaseCreditNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNotePaymentDetailDto input)
        {
            var purchaseCreditNotePaymentDetail = await _purchaseCreditNotePaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNotePaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}