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
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceVATDetails)]
    public class SalesInvoiceVATDetailsAppService : vitaAppServiceBase, ISalesInvoiceVATDetailsAppService
    {
        private readonly IRepository<SalesInvoiceVATDetail, long> _salesInvoiceVATDetailRepository;

        public SalesInvoiceVATDetailsAppService(IRepository<SalesInvoiceVATDetail, long> salesInvoiceVATDetailRepository)
        {
            _salesInvoiceVATDetailRepository = salesInvoiceVATDetailRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoiceVATDetailForViewDto>> GetAll(GetAllSalesInvoiceVATDetailsInput input)
        {

            var filteredSalesInvoiceVATDetails = _salesInvoiceVATDetailRepository.GetAll()
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

            var pagedAndFilteredSalesInvoiceVATDetails = filteredSalesInvoiceVATDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceVATDetails = from o in pagedAndFilteredSalesInvoiceVATDetails
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

            var totalCount = await filteredSalesInvoiceVATDetails.CountAsync();

            var dbList = await salesInvoiceVATDetails.ToListAsync();
            var results = new List<GetSalesInvoiceVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceVATDetailForViewDto()
                {
                    SalesInvoiceVATDetail = new SalesInvoiceVATDetailDto
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

            return new PagedResultDto<GetSalesInvoiceVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceVATDetails_Edit)]
        public async Task<GetSalesInvoiceVATDetailForEditOutput> GetSalesInvoiceVATDetailForEdit(EntityDto<long> input)
        {
            var salesInvoiceVATDetail = await _salesInvoiceVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceVATDetailForEditOutput { SalesInvoiceVATDetail = ObjectMapper.Map<CreateOrEditSalesInvoiceVATDetailDto>(salesInvoiceVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceVATDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceVATDetails_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceVATDetailDto input)
        {
            var salesInvoiceVATDetail = ObjectMapper.Map<SalesInvoiceVATDetail>(input);
            salesInvoiceVATDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                salesInvoiceVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoiceVATDetailRepository.InsertAsync(salesInvoiceVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceVATDetails_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceVATDetailDto input)
        {
            var salesInvoiceVATDetail = await _salesInvoiceVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceVATDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}