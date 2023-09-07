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
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceDiscounts)]
    public class SalesInvoiceDiscountsAppService : vitaAppServiceBase, ISalesInvoiceDiscountsAppService
    {
        private readonly IRepository<SalesInvoiceDiscount, long> _salesInvoiceDiscountRepository;

        public SalesInvoiceDiscountsAppService(IRepository<SalesInvoiceDiscount, long> salesInvoiceDiscountRepository)
        {
            _salesInvoiceDiscountRepository = salesInvoiceDiscountRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoiceDiscountForViewDto>> GetAll(GetAllSalesInvoiceDiscountsInput input)
        {

            var filteredSalesInvoiceDiscounts = _salesInvoiceDiscountRepository.GetAll()
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

            var pagedAndFilteredSalesInvoiceDiscounts = filteredSalesInvoiceDiscounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceDiscounts = from o in pagedAndFilteredSalesInvoiceDiscounts
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

            var totalCount = await filteredSalesInvoiceDiscounts.CountAsync();

            var dbList = await salesInvoiceDiscounts.ToListAsync();
            var results = new List<GetSalesInvoiceDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceDiscountForViewDto()
                {
                    SalesInvoiceDiscount = new SalesInvoiceDiscountDto
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

            return new PagedResultDto<GetSalesInvoiceDiscountForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoiceDiscountForViewDto> GetSalesInvoiceDiscountForView(long id)
        {
            var salesInvoiceDiscount = await _salesInvoiceDiscountRepository.GetAsync(id);

            var output = new GetSalesInvoiceDiscountForViewDto { SalesInvoiceDiscount = ObjectMapper.Map<SalesInvoiceDiscountDto>(salesInvoiceDiscount) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceDiscounts_Edit)]
        public async Task<GetSalesInvoiceDiscountForEditOutput> GetSalesInvoiceDiscountForEdit(EntityDto<long> input)
        {
            var salesInvoiceDiscount = await _salesInvoiceDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceDiscountForEditOutput { SalesInvoiceDiscount = ObjectMapper.Map<CreateOrEditSalesInvoiceDiscountDto>(salesInvoiceDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceDiscountDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceDiscounts_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceDiscountDto input)
        {
            var salesInvoiceDiscount = ObjectMapper.Map<SalesInvoiceDiscount>(input);
            salesInvoiceDiscount.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                salesInvoiceDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoiceDiscountRepository.InsertAsync(salesInvoiceDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceDiscounts_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceDiscountDto input)
        {
            var salesInvoiceDiscount = await _salesInvoiceDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceDiscounts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceDiscountRepository.DeleteAsync(input.Id);
        }

    }
}