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
    [AbpAuthorize(AppPermissions.Pages_VendorOwnershipDetailses)]
    public class VendorOwnershipDetailsesAppService : vitaAppServiceBase, IVendorOwnershipDetailsesAppService
    {
        private readonly IRepository<VendorOwnershipDetails, long> _vendorOwnershipDetailsRepository;

        public VendorOwnershipDetailsesAppService(IRepository<VendorOwnershipDetails, long> vendorOwnershipDetailsRepository)
        {
            _vendorOwnershipDetailsRepository = vendorOwnershipDetailsRepository;

        }

        public async Task<PagedResultDto<GetVendorOwnershipDetailsForViewDto>> GetAll(GetAllVendorOwnershipDetailsesInput input)
        {

            var filteredVendorOwnershipDetailses = _vendorOwnershipDetailsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.PartnerName.Contains(input.Filter) || e.PartnerConstitution.Contains(input.Filter) || e.PartnerNationality.Contains(input.Filter) || e.RepresentativeName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerNameFilter), e => e.PartnerName.Contains(input.PartnerNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerConstitutionFilter), e => e.PartnerConstitution.Contains(input.PartnerConstitutionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerNationalityFilter), e => e.PartnerNationality.Contains(input.PartnerNationalityFilter))
                        .WhereIf(input.MinCapitalAmountFilter != null, e => e.CapitalAmount >= input.MinCapitalAmountFilter)
                        .WhereIf(input.MaxCapitalAmountFilter != null, e => e.CapitalAmount <= input.MaxCapitalAmountFilter)
                        .WhereIf(input.MinCapitalShareFilter != null, e => e.CapitalShare >= input.MinCapitalShareFilter)
                        .WhereIf(input.MaxCapitalShareFilter != null, e => e.CapitalShare <= input.MaxCapitalShareFilter)
                        .WhereIf(input.MinProfitShareFilter != null, e => e.ProfitShare >= input.MinProfitShareFilter)
                        .WhereIf(input.MaxProfitShareFilter != null, e => e.ProfitShare <= input.MaxProfitShareFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RepresentativeNameFilter), e => e.RepresentativeName.Contains(input.RepresentativeNameFilter));

            var pagedAndFilteredVendorOwnershipDetailses = filteredVendorOwnershipDetailses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorOwnershipDetailses = from o in pagedAndFilteredVendorOwnershipDetailses
                                           select new
                                           {

                                               o.VendorID,
                                               o.VendorUniqueIdentifier,
                                               o.PartnerName,
                                               o.PartnerConstitution,
                                               o.PartnerNationality,
                                               o.CapitalAmount,
                                               o.CapitalShare,
                                               o.ProfitShare,
                                               o.RepresentativeName,
                                               Id = o.Id
                                           };

            var totalCount = await filteredVendorOwnershipDetailses.CountAsync();

            var dbList = await vendorOwnershipDetailses.ToListAsync();
            var results = new List<GetVendorOwnershipDetailsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorOwnershipDetailsForViewDto()
                {
                    VendorOwnershipDetails = new VendorOwnershipDetailsDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        PartnerName = o.PartnerName,
                        PartnerConstitution = o.PartnerConstitution,
                        PartnerNationality = o.PartnerNationality,
                        CapitalAmount = o.CapitalAmount,
                        CapitalShare = o.CapitalShare,
                        ProfitShare = o.ProfitShare,
                        RepresentativeName = o.RepresentativeName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorOwnershipDetailsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorOwnershipDetailses_Edit)]
        public async Task<GetVendorOwnershipDetailsForEditOutput> GetVendorOwnershipDetailsForEdit(EntityDto<long> input)
        {
            var vendorOwnershipDetails = await _vendorOwnershipDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorOwnershipDetailsForEditOutput { VendorOwnershipDetails = ObjectMapper.Map<CreateOrEditVendorOwnershipDetailsDto>(vendorOwnershipDetails) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorOwnershipDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorOwnershipDetailses_Create)]
        protected virtual async Task Create(CreateOrEditVendorOwnershipDetailsDto input)
        {
            var vendorOwnershipDetails = ObjectMapper.Map<VendorOwnershipDetails>(input);

            if (AbpSession.TenantId != null)
            {
                vendorOwnershipDetails.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorOwnershipDetailsRepository.InsertAsync(vendorOwnershipDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorOwnershipDetailses_Edit)]
        protected virtual async Task Update(CreateOrEditVendorOwnershipDetailsDto input)
        {
            var vendorOwnershipDetails = await _vendorOwnershipDetailsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorOwnershipDetails);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorOwnershipDetailses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorOwnershipDetailsRepository.DeleteAsync(input.Id);
        }

    }
}