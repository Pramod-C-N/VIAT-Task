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
    [AbpAuthorize(AppPermissions.Pages_VendorForeignEntities)]
    public class VendorForeignEntitiesAppService : vitaAppServiceBase, IVendorForeignEntitiesAppService
    {
        private readonly IRepository<VendorForeignEntity, long> _vendorForeignEntityRepository;

        public VendorForeignEntitiesAppService(IRepository<VendorForeignEntity, long> vendorForeignEntityRepository)
        {
            _vendorForeignEntityRepository = vendorForeignEntityRepository;

        }

        public async Task<PagedResultDto<GetVendorForeignEntityForViewDto>> GetAll(GetAllVendorForeignEntitiesInput input)
        {

            var filteredVendorForeignEntities = _vendorForeignEntityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.ForeignEntityName.Contains(input.Filter) || e.ForeignEntityAddress.Contains(input.Filter) || e.LegalRepresentative.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ForeignEntityNameFilter), e => e.ForeignEntityName.Contains(input.ForeignEntityNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ForeignEntityAddressFilter), e => e.ForeignEntityAddress.Contains(input.ForeignEntityAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalRepresentativeFilter), e => e.LegalRepresentative.Contains(input.LegalRepresentativeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.Contains(input.CountryFilter));

            var pagedAndFilteredVendorForeignEntities = filteredVendorForeignEntities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorForeignEntities = from o in pagedAndFilteredVendorForeignEntities
                                        select new
                                        {

                                            o.VendorID,
                                            o.VendorUniqueIdentifier,
                                            o.ForeignEntityName,
                                            o.ForeignEntityAddress,
                                            o.LegalRepresentative,
                                            o.Country,
                                            Id = o.Id
                                        };

            var totalCount = await filteredVendorForeignEntities.CountAsync();

            var dbList = await vendorForeignEntities.ToListAsync();
            var results = new List<GetVendorForeignEntityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorForeignEntityForViewDto()
                {
                    VendorForeignEntity = new VendorForeignEntityDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        ForeignEntityName = o.ForeignEntityName,
                        ForeignEntityAddress = o.ForeignEntityAddress,
                        LegalRepresentative = o.LegalRepresentative,
                        Country = o.Country,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorForeignEntityForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorForeignEntities_Edit)]
        public async Task<GetVendorForeignEntityForEditOutput> GetVendorForeignEntityForEdit(EntityDto<long> input)
        {
            var vendorForeignEntity = await _vendorForeignEntityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorForeignEntityForEditOutput { VendorForeignEntity = ObjectMapper.Map<CreateOrEditVendorForeignEntityDto>(vendorForeignEntity) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorForeignEntityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorForeignEntities_Create)]
        protected virtual async Task Create(CreateOrEditVendorForeignEntityDto input)
        {
            var vendorForeignEntity = ObjectMapper.Map<VendorForeignEntity>(input);

            if (AbpSession.TenantId != null)
            {
                vendorForeignEntity.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorForeignEntityRepository.InsertAsync(vendorForeignEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorForeignEntities_Edit)]
        protected virtual async Task Update(CreateOrEditVendorForeignEntityDto input)
        {
            var vendorForeignEntity = await _vendorForeignEntityRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorForeignEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorForeignEntities_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorForeignEntityRepository.DeleteAsync(input.Id);
        }

    }
}