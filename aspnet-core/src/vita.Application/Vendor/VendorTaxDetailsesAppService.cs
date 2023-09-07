using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Vendor.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Vendor
{
    [AbpAuthorize(AppPermissions.Pages_VendorTaxDetailses)]
    public class VendorTaxDetailsesAppService : vitaAppServiceBase, IVendorTaxDetailsesAppService
    {
        private readonly IRepository<VendorTaxDetails, long> _vendorTaxDetailsRepository;

        public VendorTaxDetailsesAppService(IRepository<VendorTaxDetails, long> vendorTaxDetailsRepository)
        {
            _vendorTaxDetailsRepository = vendorTaxDetailsRepository;

        }

        public async Task<PagedResultDto<GetVendorTaxDetailsForViewDto>> GetAll(GetAllVendorTaxDetailsesInput input)
        {

            var filteredVendorTaxDetailses = _vendorTaxDetailsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.BusinessCategory.Contains(input.Filter) || e.OperatingModel.Contains(input.Filter) || e.BusinessSupplies.Contains(input.Filter) || e.SalesVATCategory.Contains(input.Filter) || e.InvoiceType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessCategoryFilter), e => e.BusinessCategory.Contains(input.BusinessCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OperatingModelFilter), e => e.OperatingModel.Contains(input.OperatingModelFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessSuppliesFilter), e => e.BusinessSupplies.Contains(input.BusinessSuppliesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalesVATCategoryFilter), e => e.SalesVATCategory.Contains(input.SalesVATCategoryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceTypeFilter), e => e.InvoiceType.Contains(input.InvoiceTypeFilter));

            var pagedAndFilteredVendorTaxDetailses = filteredVendorTaxDetailses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorTaxDetailses = from o in pagedAndFilteredVendorTaxDetailses
                                     select new
                                     {

                                         o.VendorID,
                                         o.VendorUniqueIdentifier,
                                         o.BusinessCategory,
                                         o.OperatingModel,
                                         o.BusinessSupplies,
                                         o.SalesVATCategory,
                                         o.InvoiceType,
                                         Id = o.Id
                                     };

            var totalCount = await filteredVendorTaxDetailses.CountAsync();

            var dbList = await vendorTaxDetailses.ToListAsync();
            var results = new List<GetVendorTaxDetailsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorTaxDetailsForViewDto()
                {
                    VendorTaxDetails = new VendorTaxDetailsDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        BusinessCategory = o.BusinessCategory,
                        OperatingModel = o.OperatingModel,
                        BusinessSupplies = o.BusinessSupplies,
                        SalesVATCategory = o.SalesVATCategory,
                        InvoiceType = o.InvoiceType,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorTaxDetailsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorTaxDetailses_Edit)]
        public async Task<GetVendorTaxDetailsForEditOutput> GetVendorTaxDetailsForEdit(EntityDto<long> input)
        {
            var vendorTaxDetails = await _vendorTaxDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorTaxDetailsForEditOutput { VendorTaxDetails = ObjectMapper.Map<CreateOrEditVendorTaxDetailsDto>(vendorTaxDetails) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorTaxDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorTaxDetailses_Create)]
        protected virtual async Task Create(CreateOrEditVendorTaxDetailsDto input)
        {
            var vendorTaxDetails = ObjectMapper.Map<VendorTaxDetails>(input);

            if (AbpSession.TenantId != null)
            {
                vendorTaxDetails.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorTaxDetailsRepository.InsertAsync(vendorTaxDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorTaxDetailses_Edit)]
        protected virtual async Task Update(CreateOrEditVendorTaxDetailsDto input)
        {
            var vendorTaxDetails = await _vendorTaxDetailsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorTaxDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorTaxDetailses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorTaxDetailsRepository.DeleteAsync(input.Id);
        }

    }
}