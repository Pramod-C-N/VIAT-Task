using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Purchase.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryPaymentDetails)]
    public class PurchaseEntryPaymentDetailsAppService : vitaAppServiceBase, IPurchaseEntryPaymentDetailsAppService
    {
        private readonly IRepository<PurchaseEntryPaymentDetail, long> _purchaseEntryPaymentDetailRepository;

        public PurchaseEntryPaymentDetailsAppService(IRepository<PurchaseEntryPaymentDetail, long> purchaseEntryPaymentDetailRepository)
        {
            _purchaseEntryPaymentDetailRepository = purchaseEntryPaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryPaymentDetailForViewDto>> GetAll(GetAllPurchaseEntryPaymentDetailsInput input)
        {

            var filteredPurchaseEntryPaymentDetails = _purchaseEntryPaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniqueIdentifierFilter.ToString()), e => e.UniqueIdentifier.ToString() == input.UniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredPurchaseEntryPaymentDetails = filteredPurchaseEntryPaymentDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryPaymentDetails = from o in pagedAndFilteredPurchaseEntryPaymentDetails
                                              select new
                                              {

                                                  o.UniqueIdentifier,
                                                  o.IRNNo,
                                                  o.PaymentMeans,
                                                  o.CreditDebitReasonText,
                                                  o.PaymentTerms,
                                                  Id = o.Id
                                              };

            var totalCount = await filteredPurchaseEntryPaymentDetails.CountAsync();

            var dbList = await purchaseEntryPaymentDetails.ToListAsync();
            var results = new List<GetPurchaseEntryPaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryPaymentDetailForViewDto()
                {
                    PurchaseEntryPaymentDetail = new PurchaseEntryPaymentDetailDto
                    {

                        UniqueIdentifier = o.UniqueIdentifier,
                        IRNNo = o.IRNNo,
                        PaymentMeans = o.PaymentMeans,
                        CreditDebitReasonText = o.CreditDebitReasonText,
                        PaymentTerms = o.PaymentTerms,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryPaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryPaymentDetails_Edit)]
        public async Task<GetPurchaseEntryPaymentDetailForEditOutput> GetPurchaseEntryPaymentDetailForEdit(EntityDto<long> input)
        {
            var purchaseEntryPaymentDetail = await _purchaseEntryPaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryPaymentDetailForEditOutput { PurchaseEntryPaymentDetail = ObjectMapper.Map<CreateOrEditPurchaseEntryPaymentDetailDto>(purchaseEntryPaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryPaymentDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryPaymentDetails_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryPaymentDetailDto input)
        {
            var purchaseEntryPaymentDetail = ObjectMapper.Map<PurchaseEntryPaymentDetail>(input);
            purchaseEntryPaymentDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryPaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryPaymentDetailRepository.InsertAsync(purchaseEntryPaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryPaymentDetails_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryPaymentDetailDto input)
        {
            var purchaseEntryPaymentDetail = await _purchaseEntryPaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryPaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryPaymentDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryPaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}