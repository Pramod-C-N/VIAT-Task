using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Purchase.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryParties)]
    public class PurchaseEntryPartiesAppService : vitaAppServiceBase, IPurchaseEntryPartiesAppService
    {
        private readonly IRepository<PurchaseEntryParty, long> _purchaseEntryPartyRepository;

        public PurchaseEntryPartiesAppService(IRepository<PurchaseEntryParty, long> purchaseEntryPartyRepository)
        {
            _purchaseEntryPartyRepository = purchaseEntryPartyRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryPartyForViewDto>> GetAll(GetAllPurchaseEntryPartiesInput input)
        {

            var filteredPurchaseEntryParties = _purchaseEntryPartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredPurchaseEntryParties = filteredPurchaseEntryParties
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryParties = from o in pagedAndFilteredPurchaseEntryParties
                                       select new
                                       {

                                           o.IRNNo,
                                           o.RegistrationName,
                                           o.VATID,
                                           o.GroupVATID,
                                           o.CRNumber,
                                           o.OtherID,
                                           o.CustomerId,
                                           o.Type,
                                           Id = o.Id
                                       };

            var totalCount = await filteredPurchaseEntryParties.CountAsync();

            var dbList = await purchaseEntryParties.ToListAsync();
            var results = new List<GetPurchaseEntryPartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryPartyForViewDto()
                {
                    PurchaseEntryParty = new PurchaseEntryPartyDto
                    {

                        IRNNo = o.IRNNo,
                        RegistrationName = o.RegistrationName,
                        VATID = o.VATID,
                        GroupVATID = o.GroupVATID,
                        CRNumber = o.CRNumber,
                        OtherID = o.OtherID,
                        CustomerId = o.CustomerId,
                        Type = o.Type,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryPartyForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryParties_Edit)]
        public async Task<GetPurchaseEntryPartyForEditOutput> GetPurchaseEntryPartyForEdit(EntityDto<long> input)
        {
            var purchaseEntryParty = await _purchaseEntryPartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryPartyForEditOutput { PurchaseEntryParty = ObjectMapper.Map<CreateOrEditPurchaseEntryPartyDto>(purchaseEntryParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryPartyDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryParties_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryPartyDto input)
        {
            var purchaseEntryParty = ObjectMapper.Map<PurchaseEntryParty>(input);
            purchaseEntryParty.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryPartyRepository.InsertAsync(purchaseEntryParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryParties_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryPartyDto input)
        {
            var purchaseEntryParty = await _purchaseEntryPartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryParties_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryPartyRepository.DeleteAsync(input.Id);
        }

    }
}