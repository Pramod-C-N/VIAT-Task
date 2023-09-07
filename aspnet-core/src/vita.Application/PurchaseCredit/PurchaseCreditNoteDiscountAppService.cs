using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseCredit.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteDiscount)]
    public class PurchaseCreditNoteDiscountAppService : vitaAppServiceBase, IPurchaseCreditNoteDiscountAppService
    {
        private readonly IRepository<PurchaseCreditNoteDiscount, long> _purchaseCreditNoteDiscountRepository;
        private readonly IPurchaseCreditNoteDiscountExcelExporter _purchaseCreditNoteDiscountExcelExporter;

        public PurchaseCreditNoteDiscountAppService(IRepository<PurchaseCreditNoteDiscount, long> purchaseCreditNoteDiscountRepository, IPurchaseCreditNoteDiscountExcelExporter purchaseCreditNoteDiscountExcelExporter)
        {
            _purchaseCreditNoteDiscountRepository = purchaseCreditNoteDiscountRepository;
            _purchaseCreditNoteDiscountExcelExporter = purchaseCreditNoteDiscountExcelExporter;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNoteDiscountForViewDto>> GetAll(GetAllPurchaseCreditNoteDiscountInput input)
        {

            var filteredPurchaseCreditNoteDiscount = _purchaseCreditNoteDiscountRepository.GetAll()
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

            var pagedAndFilteredPurchaseCreditNoteDiscount = filteredPurchaseCreditNoteDiscount
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNoteDiscount = from o in pagedAndFilteredPurchaseCreditNoteDiscount
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

            var totalCount = await filteredPurchaseCreditNoteDiscount.CountAsync();

            var dbList = await purchaseCreditNoteDiscount.ToListAsync();
            var results = new List<GetPurchaseCreditNoteDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNoteDiscountForViewDto()
                {
                    PurchaseCreditNoteDiscount = new PurchaseCreditNoteDiscountDto
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

            return new PagedResultDto<GetPurchaseCreditNoteDiscountForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNoteDiscountForViewDto> GetPurchaseCreditNoteDiscountForView(long id)
        {
            var purchaseCreditNoteDiscount = await _purchaseCreditNoteDiscountRepository.GetAsync(id);

            var output = new GetPurchaseCreditNoteDiscountForViewDto { PurchaseCreditNoteDiscount = ObjectMapper.Map<PurchaseCreditNoteDiscountDto>(purchaseCreditNoteDiscount) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteDiscount_Edit)]
        public async Task<GetPurchaseCreditNoteDiscountForEditOutput> GetPurchaseCreditNoteDiscountForEdit(EntityDto<long> input)
        {
            var purchaseCreditNoteDiscount = await _purchaseCreditNoteDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNoteDiscountForEditOutput { PurchaseCreditNoteDiscount = ObjectMapper.Map<CreateOrEditPurchaseCreditNoteDiscountDto>(purchaseCreditNoteDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNoteDiscountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteDiscount_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNoteDiscountDto input)
        {
            var purchaseCreditNoteDiscount = ObjectMapper.Map<PurchaseCreditNoteDiscount>(input);

            if (AbpSession.TenantId != null)
            {
                purchaseCreditNoteDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNoteDiscountRepository.InsertAsync(purchaseCreditNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteDiscount_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNoteDiscountDto input)
        {
            var purchaseCreditNoteDiscount = await _purchaseCreditNoteDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNoteDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteDiscount_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNoteDiscountRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPurchaseCreditNoteDiscountToExcel(GetAllPurchaseCreditNoteDiscountForExcelInput input)
        {

            var filteredPurchaseCreditNoteDiscount = _purchaseCreditNoteDiscountRepository.GetAll()
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

            var query = (from o in filteredPurchaseCreditNoteDiscount
                         select new GetPurchaseCreditNoteDiscountForViewDto()
                         {
                             PurchaseCreditNoteDiscount = new PurchaseCreditNoteDiscountDto
                             {
                                 IRNNo = o.IRNNo,
                                 DiscountPercentage = o.DiscountPercentage,
                                 DiscountAmount = o.DiscountAmount,
                                 VATCode = o.VATCode,
                                 VATRate = o.VATRate,
                                 TaxSchemeId = o.TaxSchemeId,
                                 Id = o.Id
                             }
                         });

            var purchaseCreditNoteDiscountListDtos = await query.ToListAsync();

            return _purchaseCreditNoteDiscountExcelExporter.ExportToFile(purchaseCreditNoteDiscountListDtos);
        }

    }
}