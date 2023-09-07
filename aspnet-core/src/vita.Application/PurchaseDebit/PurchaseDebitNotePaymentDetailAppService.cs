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
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNotePaymentDetail)]
    public class PurchaseDebitNotePaymentDetailAppService : vitaAppServiceBase, IPurchaseDebitNotePaymentDetailAppService
    {
        private readonly IRepository<PurchaseDebitNotePaymentDetail, long> _purchaseDebitNotePaymentDetailRepository;

        public PurchaseDebitNotePaymentDetailAppService(IRepository<PurchaseDebitNotePaymentDetail, long> purchaseDebitNotePaymentDetailRepository)
        {
            _purchaseDebitNotePaymentDetailRepository = purchaseDebitNotePaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNotePaymentDetailForViewDto>> GetAll(GetAllPurchaseDebitNotePaymentDetailInput input)
        {

            var filteredPurchaseDebitNotePaymentDetail = _purchaseDebitNotePaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredPurchaseDebitNotePaymentDetail = filteredPurchaseDebitNotePaymentDetail
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNotePaymentDetail = from o in pagedAndFilteredPurchaseDebitNotePaymentDetail
                                                 select new
                                                 {

                                                     o.IRNNo,
                                                     o.PaymentMeans,
                                                     o.CreditDebitReasonText,
                                                     o.PaymentTerms,
                                                     Id = o.Id
                                                 };

            var totalCount = await filteredPurchaseDebitNotePaymentDetail.CountAsync();

            var dbList = await purchaseDebitNotePaymentDetail.ToListAsync();
            var results = new List<GetPurchaseDebitNotePaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNotePaymentDetailForViewDto()
                {
                    PurchaseDebitNotePaymentDetail = new PurchaseDebitNotePaymentDetailDto
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

            return new PagedResultDto<GetPurchaseDebitNotePaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseDebitNotePaymentDetailForViewDto> GetPurchaseDebitNotePaymentDetailForView(long id)
        {
            var purchaseDebitNotePaymentDetail = await _purchaseDebitNotePaymentDetailRepository.GetAsync(id);

            var output = new GetPurchaseDebitNotePaymentDetailForViewDto { PurchaseDebitNotePaymentDetail = ObjectMapper.Map<PurchaseDebitNotePaymentDetailDto>(purchaseDebitNotePaymentDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Edit)]
        public async Task<GetPurchaseDebitNotePaymentDetailForEditOutput> GetPurchaseDebitNotePaymentDetailForEdit(EntityDto<long> input)
        {
            var purchaseDebitNotePaymentDetail = await _purchaseDebitNotePaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNotePaymentDetailForEditOutput { PurchaseDebitNotePaymentDetail = ObjectMapper.Map<CreateOrEditPurchaseDebitNotePaymentDetailDto>(purchaseDebitNotePaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNotePaymentDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNotePaymentDetailDto input)
        {
            var purchaseDebitNotePaymentDetail = ObjectMapper.Map<PurchaseDebitNotePaymentDetail>(input);
            purchaseDebitNotePaymentDetail.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNotePaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNotePaymentDetailRepository.InsertAsync(purchaseDebitNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNotePaymentDetailDto input)
        {
            var purchaseDebitNotePaymentDetail = await _purchaseDebitNotePaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNotePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNotePaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}