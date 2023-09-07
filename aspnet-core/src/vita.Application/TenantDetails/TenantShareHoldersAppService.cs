using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.TenantDetails.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.TenantDetails
{
    [AbpAuthorize(AppPermissions.Pages_TenantShareHolders)]
    public class TenantShareHoldersAppService : vitaAppServiceBase, ITenantShareHoldersAppService
    {
        private readonly IRepository<TenantShareHolders> _tenantShareHoldersRepository;

        public TenantShareHoldersAppService(IRepository<TenantShareHolders> tenantShareHoldersRepository)
        {
            _tenantShareHoldersRepository = tenantShareHoldersRepository;

        }

        public async Task<PagedResultDto<GetTenantShareHoldersForViewDto>> GetAll(GetAllTenantShareHoldersInput input)
        {

            var filteredTenantShareHolders = _tenantShareHoldersRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PartnerName.Contains(input.Filter) || e.Designation.Contains(input.Filter) || e.Nationality.Contains(input.Filter) || e.CapitalAmount.Contains(input.Filter) || e.CapitalShare.Contains(input.Filter) || e.ProfitShare.Contains(input.Filter) || e.ConstitutionName.Contains(input.Filter) || e.RepresentativeName.Contains(input.Filter) || e.DomesticName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartnerNameFilter), e => e.PartnerName.Contains(input.PartnerNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DesignationFilter), e => e.Designation.Contains(input.DesignationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NationalityFilter), e => e.Nationality.Contains(input.NationalityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CapitalAmountFilter), e => e.CapitalAmount.Contains(input.CapitalAmountFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CapitalShareFilter), e => e.CapitalShare.Contains(input.CapitalShareFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfitShareFilter), e => e.ProfitShare.Contains(input.ProfitShareFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConstitutionNameFilter), e => e.ConstitutionName.Contains(input.ConstitutionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RepresentativeNameFilter), e => e.RepresentativeName.Contains(input.RepresentativeNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DomesticNameFilter), e => e.DomesticName.Contains(input.DomesticNameFilter));

            var pagedAndFilteredTenantShareHolders = filteredTenantShareHolders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantShareHolders = from o in pagedAndFilteredTenantShareHolders
                                     select new
                                     {

                                         o.PartnerName,
                                         o.Designation,
                                         o.Nationality,
                                         o.CapitalAmount,
                                         o.CapitalShare,
                                         o.ProfitShare,
                                         o.ConstitutionName,
                                         o.RepresentativeName,
                                         o.DomesticName,
                                         Id = o.Id
                                     };

            var totalCount = await filteredTenantShareHolders.CountAsync();

            var dbList = await tenantShareHolders.ToListAsync();
            var results = new List<GetTenantShareHoldersForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantShareHoldersForViewDto()
                {
                    TenantShareHolders = new TenantShareHoldersDto
                    {

                        PartnerName = o.PartnerName,
                        Designation = o.Designation,
                        Nationality = o.Nationality,
                        CapitalAmount = o.CapitalAmount,
                        CapitalShare = o.CapitalShare,
                        ProfitShare = o.ProfitShare,
                        ConstitutionName = o.ConstitutionName,
                        RepresentativeName = o.RepresentativeName,
                        DomesticName = o.DomesticName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantShareHoldersForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTenantShareHoldersForViewDto> GetTenantShareHoldersForView(int id)
        {
            var tenantShareHolders = await _tenantShareHoldersRepository.GetAsync(id);

            var output = new GetTenantShareHoldersForViewDto { TenantShareHolders = ObjectMapper.Map<TenantShareHoldersDto>(tenantShareHolders) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantShareHolders_Edit)]
        public async Task<GetTenantShareHoldersForEditOutput> GetTenantShareHoldersForEdit(EntityDto input)
        {
            var tenantShareHolders = await _tenantShareHoldersRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantShareHoldersForEditOutput { TenantShareHolders = ObjectMapper.Map<CreateOrEditTenantShareHoldersDto>(tenantShareHolders) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantShareHoldersDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantShareHolders_Create)]
        protected virtual async Task Create(CreateOrEditTenantShareHoldersDto input)
        {
            var tenantShareHolders = ObjectMapper.Map<TenantShareHolders>(input);

            if (AbpSession.TenantId != null)
            {
                tenantShareHolders.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantShareHoldersRepository.InsertAsync(tenantShareHolders);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantShareHolders_Edit)]
        protected virtual async Task Update(CreateOrEditTenantShareHoldersDto input)
        {
            var tenantShareHolders = await _tenantShareHoldersRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantShareHolders);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantShareHolders_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantShareHoldersRepository.DeleteAsync(input.Id);
        }

    }
}