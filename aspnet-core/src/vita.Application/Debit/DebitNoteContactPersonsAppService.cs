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
    [AbpAuthorize(AppPermissions.Pages_DebitNoteContactPersons)]
    public class DebitNoteContactPersonsAppService : vitaAppServiceBase, IDebitNoteContactPersonsAppService
    {
        private readonly IRepository<DebitNoteContactPerson, long> _debitNoteContactPersonRepository;

        public DebitNoteContactPersonsAppService(IRepository<DebitNoteContactPerson, long> debitNoteContactPersonRepository)
        {
            _debitNoteContactPersonRepository = debitNoteContactPersonRepository;

        }

        public async Task<PagedResultDto<GetDebitNoteContactPersonForViewDto>> GetAll(GetAllDebitNoteContactPersonsInput input)
        {

            var filteredDebitNoteContactPersons = _debitNoteContactPersonRepository.GetAll()
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

            var pagedAndFilteredDebitNoteContactPersons = filteredDebitNoteContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteContactPersons = from o in pagedAndFilteredDebitNoteContactPersons
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

            var totalCount = await filteredDebitNoteContactPersons.CountAsync();

            var dbList = await debitNoteContactPersons.ToListAsync();
            var results = new List<GetDebitNoteContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteContactPersonForViewDto()
                {
                    DebitNoteContactPerson = new DebitNoteContactPersonDto
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

            return new PagedResultDto<GetDebitNoteContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteContactPersons_Edit)]
        public async Task<GetDebitNoteContactPersonForEditOutput> GetDebitNoteContactPersonForEdit(EntityDto<long> input)
        {
            var debitNoteContactPerson = await _debitNoteContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteContactPersonForEditOutput { DebitNoteContactPerson = ObjectMapper.Map<CreateOrEditDebitNoteContactPersonDto>(debitNoteContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteContactPersonDto input)
        {
                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteContactPersonDto input)
        {
            var debitNoteContactPerson = ObjectMapper.Map<DebitNoteContactPerson>(input);
            debitNoteContactPerson.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNoteContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteContactPersonRepository.InsertAsync(debitNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteContactPersonDto input)
        {
            var debitNoteContactPerson = await _debitNoteContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteContactPersons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}