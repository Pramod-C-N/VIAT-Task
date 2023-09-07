using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Debit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Debit
{
    [AbpAuthorize(AppPermissions.Pages_DebitNoteParties)]
    public class DebitNotePartiesAppService : vitaAppServiceBase, IDebitNotePartiesAppService
    {
        private readonly IRepository<DebitNoteParty, long> _debitNotePartyRepository;

        public DebitNotePartiesAppService(IRepository<DebitNoteParty, long> debitNotePartyRepository)
        {
            _debitNotePartyRepository = debitNotePartyRepository;

        }

        public async Task<PagedResultDto<GetDebitNotePartyForViewDto>> GetAll(GetAllDebitNotePartiesInput input)
        {

            var filteredDebitNoteParties = _debitNotePartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredDebitNoteParties = filteredDebitNoteParties
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteParties = from o in pagedAndFilteredDebitNoteParties
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

            var totalCount = await filteredDebitNoteParties.CountAsync();

            var dbList = await debitNoteParties.ToListAsync();
            var results = new List<GetDebitNotePartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNotePartyForViewDto()
                {
                    DebitNoteParty = new DebitNotePartyDto
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

            return new PagedResultDto<GetDebitNotePartyForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteParties_Edit)]
        public async Task<GetDebitNotePartyForEditOutput> GetDebitNotePartyForEdit(EntityDto<long> input)
        {
            var debitNoteParty = await _debitNotePartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNotePartyForEditOutput { DebitNoteParty = ObjectMapper.Map<CreateOrEditDebitNotePartyDto>(debitNoteParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNotePartyDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteParties_Create)]
        protected virtual async Task Create(CreateOrEditDebitNotePartyDto input)
        {
            var debitNoteParty = ObjectMapper.Map<DebitNoteParty>(input);
            debitNoteParty.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNoteParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNotePartyRepository.InsertAsync(debitNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteParties_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNotePartyDto input)
        {
            var debitNoteParty = await _debitNotePartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteParties_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNotePartyRepository.DeleteAsync(input.Id);
        }

    }
}