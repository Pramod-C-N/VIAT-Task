using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseDebit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseDebit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteParty)]
    public class PurchaseDebitNotePartyAppService : vitaAppServiceBase, IPurchaseDebitNotePartyAppService
    {
        private readonly IRepository<PurchaseDebitNoteParty, long> _purchaseDebitNotePartyRepository;

        public PurchaseDebitNotePartyAppService(IRepository<PurchaseDebitNoteParty, long> purchaseDebitNotePartyRepository)
        {
            _purchaseDebitNotePartyRepository = purchaseDebitNotePartyRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNotePartyForViewDto>> GetAll(GetAllPurchaseDebitNotePartyInput input)
        {

            var filteredPurchaseDebitNoteParty = _purchaseDebitNotePartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredPurchaseDebitNoteParty = filteredPurchaseDebitNoteParty
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteParty = from o in pagedAndFilteredPurchaseDebitNoteParty
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

            var totalCount = await filteredPurchaseDebitNoteParty.CountAsync();

            var dbList = await purchaseDebitNoteParty.ToListAsync();
            var results = new List<GetPurchaseDebitNotePartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNotePartyForViewDto()
                {
                    PurchaseDebitNoteParty = new PurchaseDebitNotePartyDto
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

            return new PagedResultDto<GetPurchaseDebitNotePartyForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteParty_Edit)]
        public async Task<GetPurchaseDebitNotePartyForEditOutput> GetPurchaseDebitNotePartyForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteParty = await _purchaseDebitNotePartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNotePartyForEditOutput { PurchaseDebitNoteParty = ObjectMapper.Map<CreateOrEditPurchaseDebitNotePartyDto>(purchaseDebitNoteParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNotePartyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteParty_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNotePartyDto input)
        {
            var purchaseDebitNoteParty = ObjectMapper.Map<PurchaseDebitNoteParty>(input);
            purchaseDebitNoteParty.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNotePartyRepository.InsertAsync(purchaseDebitNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteParty_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNotePartyDto input)
        {
            var purchaseDebitNoteParty = await _purchaseDebitNotePartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteParty_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNotePartyRepository.DeleteAsync(input.Id);
        }

    }
}