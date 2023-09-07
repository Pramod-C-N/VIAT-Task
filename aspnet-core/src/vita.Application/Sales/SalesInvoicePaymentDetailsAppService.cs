using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Sales
{
    [AbpAuthorize(AppPermissions.Pages_SalesInvoicePaymentDetails)]
    public class SalesInvoicePaymentDetailsAppService : vitaAppServiceBase, ISalesInvoicePaymentDetailsAppService
    {
        private readonly IRepository<SalesInvoicePaymentDetail, long> _salesInvoicePaymentDetailRepository;

        public SalesInvoicePaymentDetailsAppService(IRepository<SalesInvoicePaymentDetail, long> salesInvoicePaymentDetailRepository)
        {
            _salesInvoicePaymentDetailRepository = salesInvoicePaymentDetailRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoicePaymentDetailForViewDto>> GetAll(GetAllSalesInvoicePaymentDetailsInput input)
        {

            var filteredSalesInvoicePaymentDetails = _salesInvoicePaymentDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.PaymentMeans.Contains(input.Filter) || e.CreditDebitReasonText.Contains(input.Filter) || e.PaymentTerms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentMeansFilter), e => e.PaymentMeans.Contains(input.PaymentMeansFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreditDebitReasonTextFilter), e => e.CreditDebitReasonText.Contains(input.CreditDebitReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTermsFilter), e => e.PaymentTerms.Contains(input.PaymentTermsFilter));

            var pagedAndFilteredSalesInvoicePaymentDetails = filteredSalesInvoicePaymentDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoicePaymentDetails = from o in pagedAndFilteredSalesInvoicePaymentDetails
                                             select new
                                             {

                                                 o.IRNNo,
                                                 o.PaymentMeans,
                                                 o.CreditDebitReasonText,
                                                 o.PaymentTerms,
                                                 Id = o.Id
                                             };

            var totalCount = await filteredSalesInvoicePaymentDetails.CountAsync();

            var dbList = await salesInvoicePaymentDetails.ToListAsync();
            var results = new List<GetSalesInvoicePaymentDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoicePaymentDetailForViewDto()
                {
                    SalesInvoicePaymentDetail = new SalesInvoicePaymentDetailDto
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

            return new PagedResultDto<GetSalesInvoicePaymentDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoicePaymentDetailForViewDto> GetSalesInvoicePaymentDetailForView(long id)
        {
            var salesInvoicePaymentDetail = await _salesInvoicePaymentDetailRepository.GetAsync(id);

            var output = new GetSalesInvoicePaymentDetailForViewDto { SalesInvoicePaymentDetail = ObjectMapper.Map<SalesInvoicePaymentDetailDto>(salesInvoicePaymentDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoicePaymentDetails_Edit)]
        public async Task<GetSalesInvoicePaymentDetailForEditOutput> GetSalesInvoicePaymentDetailForEdit(EntityDto<long> input)
        {
            var salesInvoicePaymentDetail = await _salesInvoicePaymentDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoicePaymentDetailForEditOutput { SalesInvoicePaymentDetail = ObjectMapper.Map<CreateOrEditSalesInvoicePaymentDetailDto>(salesInvoicePaymentDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoicePaymentDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoicePaymentDetails_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoicePaymentDetailDto input)
        {
            var salesInvoicePaymentDetail = ObjectMapper.Map<SalesInvoicePaymentDetail>(input);
            salesInvoicePaymentDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                salesInvoicePaymentDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoicePaymentDetailRepository.InsertAsync(salesInvoicePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoicePaymentDetails_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoicePaymentDetailDto input)
        {
            var salesInvoicePaymentDetail = await _salesInvoicePaymentDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoicePaymentDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoicePaymentDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoicePaymentDetailRepository.DeleteAsync(input.Id);
        }

    }
}