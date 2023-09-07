using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.DraftFee.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using IdentityServer4.Models;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftParties)]
    public class DraftPartiesAppService : vitaAppServiceBase, IDraftPartiesAppService
    {
        private readonly IRepository<DraftParty, long> _draftPartyRepository;

        public DraftPartiesAppService(IRepository<DraftParty, long> draftPartyRepository)
        {
            _draftPartyRepository = draftPartyRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftPartyForViewDto>> GetAll(GetAllDraftPartiesInput input)
        {

            var filteredDraftParties = _draftPartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.Language.Contains(input.Filter) || e.OtherDocumentTypeId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherDocumentTypeIdFilter), e => e.OtherDocumentTypeId.Contains(input.OtherDocumentTypeIdFilter));

            var pagedAndFilteredDraftParties = filteredDraftParties
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftParties = from o in pagedAndFilteredDraftParties
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
                                   o.AdditionalData1,
                                   o.Language,
                                   o.OtherDocumentTypeId,
                                   Id = o.Id
                               };

            var totalCount = await filteredDraftParties.CountAsync();

            var dbList = await draftParties.ToListAsync();
            var results = new List<GetDraftPartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftPartyForViewDto()
                {
                    DraftParty = new DraftPartyDto
                    {

                        IRNNo = o.IRNNo,
                        RegistrationName = o.RegistrationName,
                        VATID = o.VATID,
                        GroupVATID = o.GroupVATID,
                        CRNumber = o.CRNumber,
                        OtherID = o.OtherID,
                        CustomerId = o.CustomerId,
                        Type = o.Type,
                        AdditionalData1 = o.AdditionalData1,
                        Language = o.Language,
                        OtherDocumentTypeId = o.OtherDocumentTypeId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftPartyForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftPartyForViewDto> GetDraftPartyForView(long id)
        {
            var draftParty = await _draftPartyRepository.GetAsync(id);

            var output = new GetDraftPartyForViewDto { DraftParty = ObjectMapper.Map<DraftPartyDto>(draftParty) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftParties_Edit)]
        public virtual async Task<GetDraftPartyForEditOutput> GetDraftPartyForEdit(EntityDto<long> input)
        {
            var draftParty = await _draftPartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftPartyForEditOutput { DraftParty = ObjectMapper.Map<CreateOrEditDraftPartyDto>(draftParty) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftPartyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftParties_Create)]
        protected virtual async Task Create(CreateOrEditDraftPartyDto input)
        {
            var draftParty = ObjectMapper.Map<DraftParty>(input);
            draftParty.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                draftParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftPartyRepository.InsertAsync(draftParty);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftParties_Edit)]
        protected virtual async Task Update(CreateOrEditDraftPartyDto input)
        {
            var draftParty = await _draftPartyRepository.FirstOrDefaultAsync((long)input.Id);
            draftParty.UniqueIdentifier = Guid.NewGuid();
            ObjectMapper.Map(input, draftParty);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftParties_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftPartyRepository.DeleteAsync(input.Id);
        }

    }
}