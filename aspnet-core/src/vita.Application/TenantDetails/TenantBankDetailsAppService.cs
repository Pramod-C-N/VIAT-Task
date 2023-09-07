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
    [AbpAuthorize(AppPermissions.Pages_TenantBankDetails)]
    public class TenantBankDetailsAppService : vitaAppServiceBase, ITenantBankDetailsAppService
    {
        private readonly IRepository<TenantBankDetail> _tenantBankDetailRepository;

        public TenantBankDetailsAppService(IRepository<TenantBankDetail> tenantBankDetailRepository)
        {
            _tenantBankDetailRepository = tenantBankDetailRepository;

        }

        public async Task<PagedResultDto<GetTenantBankDetailForViewDto>> GetAll(GetAllTenantBankDetailsInput input)
        {

            var filteredTenantBankDetails = _tenantBankDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountName.Contains(input.Filter) || e.AccountNumber.Contains(input.Filter) || e.IBAN.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.SwiftCode.Contains(input.Filter) || e.BranchName.Contains(input.Filter) || e.BranchAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniqueIdentifierFilter.ToString()), e => e.UniqueIdentifier.ToString() == input.UniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.Contains(input.AccountNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNumberFilter), e => e.AccountNumber.Contains(input.AccountNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IBANFilter), e => e.IBAN.Contains(input.IBANFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName.Contains(input.BankNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SwiftCodeFilter), e => e.SwiftCode.Contains(input.SwiftCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNameFilter), e => e.BranchName.Contains(input.BranchNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BranchAddressFilter), e => e.BranchAddress.Contains(input.BranchAddressFilter));

            var pagedAndFilteredTenantBankDetails = filteredTenantBankDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantBankDetails = from o in pagedAndFilteredTenantBankDetails
                                    select new
                                    {

                                        o.UniqueIdentifier,
                                        o.AccountName,
                                        o.AccountNumber,
                                        o.IBAN,
                                        o.BankName,
                                        o.SwiftCode,
                                        o.IsActive,
                                        o.BranchName,
                                        o.BranchAddress,
                                        Id = o.Id
                                    };

            var totalCount = await filteredTenantBankDetails.CountAsync();

            var dbList = await tenantBankDetails.ToListAsync();
            var results = new List<GetTenantBankDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantBankDetailForViewDto()
                {
                    TenantBankDetail = new TenantBankDetailDto
                    {

                        UniqueIdentifier = o.UniqueIdentifier,
                        AccountName = o.AccountName,
                        AccountNumber = o.AccountNumber,
                        IBAN = o.IBAN,
                        BankName = o.BankName,
                        SwiftCode = o.SwiftCode,
                        IsActive = o.IsActive,
                        BranchName = o.BranchName,
                        BranchAddress = o.BranchAddress,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantBankDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBankDetails_Edit)]
        public async Task<GetTenantBankDetailForEditOutput> GetTenantBankDetailForEdit(EntityDto input)
        {
            var tenantBankDetail = await _tenantBankDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantBankDetailForEditOutput { TenantBankDetail = ObjectMapper.Map<CreateOrEditTenantBankDetailDto>(tenantBankDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantBankDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantBankDetails_Create)]
        protected virtual async Task Create(CreateOrEditTenantBankDetailDto input)
        {
            var tenantBankDetail = ObjectMapper.Map<TenantBankDetail>(input);

            if (AbpSession.TenantId != null)
            {
                tenantBankDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantBankDetailRepository.InsertAsync(tenantBankDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBankDetails_Edit)]
        protected virtual async Task Update(CreateOrEditTenantBankDetailDto input)
        {
            var tenantBankDetail = await _tenantBankDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantBankDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBankDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantBankDetailRepository.DeleteAsync(input.Id);
        }

    }
}