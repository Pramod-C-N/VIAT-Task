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
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceSummaries)]
    public class SalesInvoiceSummariesAppService : vitaAppServiceBase, ISalesInvoiceSummariesAppService
    {
        private readonly IRepository<SalesInvoiceSummary, long> _salesInvoiceSummaryRepository;

        public SalesInvoiceSummariesAppService(IRepository<SalesInvoiceSummary, long> salesInvoiceSummaryRepository)
        {
            _salesInvoiceSummaryRepository = salesInvoiceSummaryRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoiceSummaryForViewDto>> GetAll(GetAllSalesInvoiceSummariesInput input)
        {

            var filteredSalesInvoiceSummaries = _salesInvoiceSummaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.NetInvoiceAmountCurrency.Contains(input.Filter) || e.SumOfInvoiceLineNetAmountCurrency.Contains(input.Filter) || e.TotalAmountWithoutVATCurrency.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.PaidAmountCurrency.Contains(input.Filter) || e.PayableAmountCurrency.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(input.MinNetInvoiceAmountFilter != null, e => e.NetInvoiceAmount >= input.MinNetInvoiceAmountFilter)
                        .WhereIf(input.MaxNetInvoiceAmountFilter != null, e => e.NetInvoiceAmount <= input.MaxNetInvoiceAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NetInvoiceAmountCurrencyFilter), e => e.NetInvoiceAmountCurrency.Contains(input.NetInvoiceAmountCurrencyFilter))
                        .WhereIf(input.MinSumOfInvoiceLineNetAmountFilter != null, e => e.SumOfInvoiceLineNetAmount >= input.MinSumOfInvoiceLineNetAmountFilter)
                        .WhereIf(input.MaxSumOfInvoiceLineNetAmountFilter != null, e => e.SumOfInvoiceLineNetAmount <= input.MaxSumOfInvoiceLineNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SumOfInvoiceLineNetAmountCurrencyFilter), e => e.SumOfInvoiceLineNetAmountCurrency.Contains(input.SumOfInvoiceLineNetAmountCurrencyFilter))
                        .WhereIf(input.MinTotalAmountWithoutVATFilter != null, e => e.TotalAmountWithoutVAT >= input.MinTotalAmountWithoutVATFilter)
                        .WhereIf(input.MaxTotalAmountWithoutVATFilter != null, e => e.TotalAmountWithoutVAT <= input.MaxTotalAmountWithoutVATFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalAmountWithoutVATCurrencyFilter), e => e.TotalAmountWithoutVATCurrency.Contains(input.TotalAmountWithoutVATCurrencyFilter))
                        .WhereIf(input.MinTotalVATAmountFilter != null, e => e.TotalVATAmount >= input.MinTotalVATAmountFilter)
                        .WhereIf(input.MaxTotalVATAmountFilter != null, e => e.TotalVATAmount <= input.MaxTotalVATAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter))
                        .WhereIf(input.MinTotalAmountWithVATFilter != null, e => e.TotalAmountWithVAT >= input.MinTotalAmountWithVATFilter)
                        .WhereIf(input.MaxTotalAmountWithVATFilter != null, e => e.TotalAmountWithVAT <= input.MaxTotalAmountWithVATFilter)
                        .WhereIf(input.MinPaidAmountFilter != null, e => e.PaidAmount >= input.MinPaidAmountFilter)
                        .WhereIf(input.MaxPaidAmountFilter != null, e => e.PaidAmount <= input.MaxPaidAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaidAmountCurrencyFilter), e => e.PaidAmountCurrency.Contains(input.PaidAmountCurrencyFilter))
                        .WhereIf(input.MinPayableAmountFilter != null, e => e.PayableAmount >= input.MinPayableAmountFilter)
                        .WhereIf(input.MaxPayableAmountFilter != null, e => e.PayableAmount <= input.MaxPayableAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayableAmountCurrencyFilter), e => e.PayableAmountCurrency.Contains(input.PayableAmountCurrencyFilter))
                        .WhereIf(input.MinAdvanceAmountwithoutVatFilter != null, e => e.AdvanceAmountwithoutVat >= input.MinAdvanceAmountwithoutVatFilter)
                        .WhereIf(input.MaxAdvanceAmountwithoutVatFilter != null, e => e.AdvanceAmountwithoutVat <= input.MaxAdvanceAmountwithoutVatFilter)
                        .WhereIf(input.MinAdvanceVatFilter != null, e => e.AdvanceVat >= input.MinAdvanceVatFilter)
                        .WhereIf(input.MaxAdvanceVatFilter != null, e => e.AdvanceVat <= input.MaxAdvanceVatFilter);

            var pagedAndFilteredSalesInvoiceSummaries = filteredSalesInvoiceSummaries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceSummaries = from o in pagedAndFilteredSalesInvoiceSummaries
                                        select new
                                        {

                                            o.IRNNo,
                                            o.NetInvoiceAmount,
                                            o.NetInvoiceAmountCurrency,
                                            o.SumOfInvoiceLineNetAmount,
                                            o.SumOfInvoiceLineNetAmountCurrency,
                                            o.TotalAmountWithoutVAT,
                                            o.TotalAmountWithoutVATCurrency,
                                            o.TotalVATAmount,
                                            o.CurrencyCode,
                                            o.TotalAmountWithVAT,
                                            o.PaidAmount,
                                            o.PaidAmountCurrency,
                                            o.PayableAmount,
                                            o.PayableAmountCurrency,
                                            o.AdvanceAmountwithoutVat,
                                            o.AdvanceVat,
                                            Id = o.Id
                                        };

            var totalCount = await filteredSalesInvoiceSummaries.CountAsync();

            var dbList = await salesInvoiceSummaries.ToListAsync();
            var results = new List<GetSalesInvoiceSummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceSummaryForViewDto()
                {
                    SalesInvoiceSummary = new SalesInvoiceSummaryDto
                    {

                        IRNNo = o.IRNNo,
                        NetInvoiceAmount = o.NetInvoiceAmount,
                        NetInvoiceAmountCurrency = o.NetInvoiceAmountCurrency,
                        SumOfInvoiceLineNetAmount = o.SumOfInvoiceLineNetAmount,
                        SumOfInvoiceLineNetAmountCurrency = o.SumOfInvoiceLineNetAmountCurrency,
                        TotalAmountWithoutVAT = o.TotalAmountWithoutVAT,
                        TotalAmountWithoutVATCurrency = o.TotalAmountWithoutVATCurrency,
                        TotalVATAmount = o.TotalVATAmount,
                        CurrencyCode = o.CurrencyCode,
                        TotalAmountWithVAT = o.TotalAmountWithVAT,
                        PaidAmount = o.PaidAmount,
                        PaidAmountCurrency = o.PaidAmountCurrency,
                        PayableAmount = o.PayableAmount,
                        PayableAmountCurrency = o.PayableAmountCurrency,
                        AdvanceAmountwithoutVat = o.AdvanceAmountwithoutVat,
                        AdvanceVat = o.AdvanceVat,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSalesInvoiceSummaryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoiceSummaryForViewDto> GetSalesInvoiceSummaryForView(long id)
        {
            var salesInvoiceSummary = await _salesInvoiceSummaryRepository.GetAsync(id);

            var output = new GetSalesInvoiceSummaryForViewDto { SalesInvoiceSummary = ObjectMapper.Map<SalesInvoiceSummaryDto>(salesInvoiceSummary) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceSummaries_Edit)]
        public async Task<GetSalesInvoiceSummaryForEditOutput> GetSalesInvoiceSummaryForEdit(EntityDto<long> input)
        {
            var salesInvoiceSummary = await _salesInvoiceSummaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceSummaryForEditOutput { SalesInvoiceSummary = ObjectMapper.Map<CreateOrEditSalesInvoiceSummaryDto>(salesInvoiceSummary) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceSummaryDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceSummaries_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceSummaryDto input)
        {
            var salesInvoiceSummary = ObjectMapper.Map<SalesInvoiceSummary>(input);
            salesInvoiceSummary.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                salesInvoiceSummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoiceSummaryRepository.InsertAsync(salesInvoiceSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceSummaries_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceSummaryDto input)
        {
            var salesInvoiceSummary = await _salesInvoiceSummaryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceSummaries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceSummaryRepository.DeleteAsync(input.Id);
        }

    }
}