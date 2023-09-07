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
    [AbpAuthorize(AppPermissions.Pages_VendorSectorDetails)]
    public class VendorSectorDetailsAppService : vitaAppServiceBase, IVendorSectorDetailsAppService
    {
        private readonly IRepository<VendorSectorDetail, long> _vendorSectorDetailRepository;

        public VendorSectorDetailsAppService(IRepository<VendorSectorDetail, long> vendorSectorDetailRepository)
        {
            _vendorSectorDetailRepository = vendorSectorDetailRepository;

        }

        public async Task<PagedResultDto<GetVendorSectorDetailForViewDto>> GetAll(GetAllVendorSectorDetailsInput input)
        {

            var filteredVendorSectorDetails = _vendorSectorDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.SubIndustryCode.Contains(input.Filter) || e.SubIndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryCodeFilter), e => e.SubIndustryCode.Contains(input.SubIndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryNameFilter), e => e.SubIndustryName.Contains(input.SubIndustryNameFilter));

            var pagedAndFilteredVendorSectorDetails = filteredVendorSectorDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorSectorDetails = from o in pagedAndFilteredVendorSectorDetails
                                      select new
                                      {

                                          o.VendorID,
                                          o.VendorUniqueIdentifier,
                                          o.SubIndustryCode,
                                          o.SubIndustryName,
                                          Id = o.Id
                                      };

            var totalCount = await filteredVendorSectorDetails.CountAsync();

            var dbList = await vendorSectorDetails.ToListAsync();
            var results = new List<GetVendorSectorDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorSectorDetailForViewDto()
                {
                    VendorSectorDetail = new VendorSectorDetailDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        SubIndustryCode = o.SubIndustryCode,
                        SubIndustryName = o.SubIndustryName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorSectorDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorSectorDetails_Edit)]
        public async Task<GetVendorSectorDetailForEditOutput> GetVendorSectorDetailForEdit(EntityDto<long> input)
        {
            var vendorSectorDetail = await _vendorSectorDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorSectorDetailForEditOutput { VendorSectorDetail = ObjectMapper.Map<CreateOrEditVendorSectorDetailDto>(vendorSectorDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorSectorDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorSectorDetails_Create)]
        protected virtual async Task Create(CreateOrEditVendorSectorDetailDto input)
        {
            var vendorSectorDetail = ObjectMapper.Map<VendorSectorDetail>(input);

            if (AbpSession.TenantId != null)
            {
                vendorSectorDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorSectorDetailRepository.InsertAsync(vendorSectorDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorSectorDetails_Edit)]
        protected virtual async Task Update(CreateOrEditVendorSectorDetailDto input)
        {
            var vendorSectorDetail = await _vendorSectorDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorSectorDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorSectorDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorSectorDetailRepository.DeleteAsync(input.Id);
        }

    }
}