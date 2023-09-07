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
    [AbpAuthorize(AppPermissions.Pages_CreditNoteContactPerson)]
    public class CreditNoteContactPersonAppService : vitaAppServiceBase, ICreditNoteContactPersonAppService
    {
        private readonly IRepository<CreditNoteContactPerson, long> _creditNoteContactPersonRepository;

        public CreditNoteContactPersonAppService(IRepository<CreditNoteContactPerson, long> creditNoteContactPersonRepository)
        {
            _creditNoteContactPersonRepository = creditNoteContactPersonRepository;

        }

        public async Task<PagedResultDto<GetCreditNoteContactPersonForViewDto>> GetAll(GetAllCreditNoteContactPersonInput input)
        {

            var filteredCreditNoteContactPerson = _creditNoteContactPersonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.EmployeeCode.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.GovtId.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeCodeFilter), e => e.EmployeeCode.Contains(input.EmployeeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovtIdFilter), e => e.GovtId.Contains(input.GovtIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredCreditNoteContactPerson = filteredCreditNoteContactPerson
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteContactPerson = from o in pagedAndFilteredCreditNoteContactPerson
                                          select new
                                          {

                                              o.IRNNo,
                                              o.Name,
                                              o.EmployeeCode,
                                              o.ContactNumber,
                                              o.GovtId,
                                              o.Email,
                                              o.Address,
                                              o.Location,
                                              o.Type,
                                              Id = o.Id
                                          };

            var totalCount = await filteredCreditNoteContactPerson.CountAsync();

            var dbList = await creditNoteContactPerson.ToListAsync();
            var results = new List<GetCreditNoteContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteContactPersonForViewDto()
                {
                    CreditNoteContactPerson = new CreditNoteContactPersonDto
                    {

                        IRNNo = o.IRNNo,
                        Name = o.Name,
                        EmployeeCode = o.EmployeeCode,
                        ContactNumber = o.ContactNumber,
                        GovtId = o.GovtId,
                        Email = o.Email,
                        Address = o.Address,
                        Location = o.Location,
                        Type = o.Type,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCreditNoteContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteContactPerson_Edit)]
        public async Task<GetCreditNoteContactPersonForEditOutput> GetCreditNoteContactPersonForEdit(EntityDto<long> input)
        {
            var creditNoteContactPerson = await _creditNoteContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteContactPersonForEditOutput { CreditNoteContactPerson = ObjectMapper.Map<CreateOrEditCreditNoteContactPersonDto>(creditNoteContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCreditNoteContactPersonDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteContactPerson_Create)]
        protected virtual async Task Create(CreateOrEditCreditNoteContactPersonDto input)
        {
            var creditNoteContactPerson = ObjectMapper.Map<CreditNoteContactPerson>(input);
            creditNoteContactPerson.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNoteContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNoteContactPersonRepository.InsertAsync(creditNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteContactPerson_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteContactPersonDto input)
        {
            var creditNoteContactPerson = await _creditNoteContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, creditNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteContactPerson_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _creditNoteContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}