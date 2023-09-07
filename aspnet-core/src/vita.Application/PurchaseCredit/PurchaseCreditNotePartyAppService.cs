using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteParty)]
    public class PurchaseCreditNotePartyAppService : vitaAppServiceBase, IPurchaseCreditNotePartyAppService
    {
        private readonly IRepository<PurchaseCreditNoteParty, long> _purchaseCreditNotePartyRepository;

        public PurchaseCreditNotePartyAppService(IRepository<PurchaseCreditNoteParty, long> purchaseCreditNotePartyRepository)
        {
            _purchaseCreditNotePartyRepository = purchaseCreditNotePartyRepository;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNotePartyForViewDto>> GetAll(GetAllPurchaseCreditNotePartyInput input)
        {

            var filteredPurchaseCreditNoteParty = _purchaseCreditNotePartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredPurchaseCreditNoteParty = filteredPurchaseCreditNoteParty
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNoteParty = from o in pagedAndFilteredPurchaseCreditNoteParty
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

            var totalCount = await filteredPurchaseCreditNoteParty.CountAsync();

            var dbList = await purchaseCreditNoteParty.ToListAsync();
            var results = new List<GetPurchaseCreditNotePartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNotePartyForViewDto()
                {
                    PurchaseCreditNoteParty = new PurchaseCreditNotePartyDto
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

            return new PagedResultDto<GetPurchaseCreditNotePartyForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNotePartyForViewDto> GetPurchaseCreditNotePartyForView(long id)
        {
            var purchaseCreditNoteParty = await _purchaseCreditNotePartyRepository.GetAsync(id);

            var output = new GetPurchaseCreditNotePartyForViewDto { PurchaseCreditNoteParty = ObjectMapper.Map<PurchaseCreditNotePartyDto>(purchaseCreditNoteParty) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteParty_Edit)]
        public async Task<GetPurchaseCreditNotePartyForEditOutput> GetPurchaseCreditNotePartyForEdit(EntityDto<long> input)
        {
            var purchaseCreditNoteParty = await _purchaseCreditNotePartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNotePartyForEditOutput { PurchaseCreditNoteParty = ObjectMapper.Map<CreateOrEditPurchaseCreditNotePartyDto>(purchaseCreditNoteParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNotePartyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteParty_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNotePartyDto input)
        {
            var purchaseCreditNoteParty = ObjectMapper.Map<PurchaseCreditNoteParty>(input);
            purchaseCreditNoteParty.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseCreditNoteParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNotePartyRepository.InsertAsync(purchaseCreditNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteParty_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNotePartyDto input)
        {
            var purchaseCreditNoteParty = await _purchaseCreditNotePartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteParty_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNotePartyRepository.DeleteAsync(input.Id);
        }

    }
}