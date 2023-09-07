using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Credit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Credit
{
    [AbpAuthorize(AppPermissions.Pages_CreditNoteParty)]
    public class CreditNotePartyAppService : vitaAppServiceBase, ICreditNotePartyAppService
    {
        private readonly IRepository<CreditNoteParty, long> _creditNotePartyRepository;

        public CreditNotePartyAppService(IRepository<CreditNoteParty, long> creditNotePartyRepository)
        {
            _creditNotePartyRepository = creditNotePartyRepository;

        }

        public async Task<PagedResultDto<GetCreditNotePartyForViewDto>> GetAll(GetAllCreditNotePartyInput input)
        {

            var filteredCreditNoteParty = _creditNotePartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredCreditNoteParty = filteredCreditNoteParty
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteParty = from o in pagedAndFilteredCreditNoteParty
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

            var totalCount = await filteredCreditNoteParty.CountAsync();

            var dbList = await creditNoteParty.ToListAsync();
            var results = new List<GetCreditNotePartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNotePartyForViewDto()
                {
                    CreditNoteParty = new CreditNotePartyDto
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

            return new PagedResultDto<GetCreditNotePartyForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteParty_Edit)]
        public async Task<GetCreditNotePartyForEditOutput> GetCreditNotePartyForEdit(EntityDto<long> input)
        {
            var creditNoteParty = await _creditNotePartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNotePartyForEditOutput { CreditNoteParty = ObjectMapper.Map<CreateOrEditCreditNotePartyDto>(creditNoteParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNotePartyDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteParty_Create)]
        protected virtual async Task Create(CreateOrEditCreditNotePartyDto input)
        {
            var creditNoteParty = ObjectMapper.Map<CreditNoteParty>(input);
            creditNoteParty.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNoteParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNotePartyRepository.InsertAsync(creditNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteParty_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNotePartyDto input)
        {
            var creditNoteParty = await _creditNotePartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNoteParty);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteParty_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNotePartyRepository.DeleteAsync(input.Id);
        }

    }
}