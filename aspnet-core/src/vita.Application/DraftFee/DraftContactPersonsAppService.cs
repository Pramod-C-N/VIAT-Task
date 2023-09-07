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

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftContactPersons)]
    public class DraftContactPersonsAppService : vitaAppServiceBase, IDraftContactPersonsAppService
    {
        private readonly IRepository<DraftContactPerson, long> _draftContactPersonRepository;

        public DraftContactPersonsAppService(IRepository<DraftContactPerson, long> draftContactPersonRepository)
        {
            _draftContactPersonRepository = draftContactPersonRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftContactPersonForViewDto>> GetAll(GetAllDraftContactPersonsInput input)
        {

            var filteredDraftContactPersons = _draftContactPersonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.EmployeeCode.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.GovtId.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.Language.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeCodeFilter), e => e.EmployeeCode.Contains(input.EmployeeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovtIdFilter), e => e.GovtId.Contains(input.GovtIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter));

            var pagedAndFilteredDraftContactPersons = filteredDraftContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftContactPersons = from o in pagedAndFilteredDraftContactPersons
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
                                          o.AdditionalData1,
                                          o.Language,
                                          Id = o.Id
                                      };

            var totalCount = await filteredDraftContactPersons.CountAsync();

            var dbList = await draftContactPersons.ToListAsync();
            var results = new List<GetDraftContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftContactPersonForViewDto()
                {
                    DraftContactPerson = new DraftContactPersonDto
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
                        AdditionalData1 = o.AdditionalData1,
                        Language = o.Language,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftContactPersonForViewDto> GetDraftContactPersonForView(long id)
        {
            var draftContactPerson = await _draftContactPersonRepository.GetAsync(id);

            var output = new GetDraftContactPersonForViewDto { DraftContactPerson = ObjectMapper.Map<DraftContactPersonDto>(draftContactPerson) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftContactPersons_Edit)]
        public virtual async Task<GetDraftContactPersonForEditOutput> GetDraftContactPersonForEdit(EntityDto<long> input)
        {
            var draftContactPerson = await _draftContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftContactPersonForEditOutput { DraftContactPerson = ObjectMapper.Map<CreateOrEditDraftContactPersonDto>(draftContactPerson) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftContactPersonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditDraftContactPersonDto input)
        {
            var draftContactPerson = ObjectMapper.Map<DraftContactPerson>(input);
            draftContactPerson.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                draftContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftContactPersonRepository.InsertAsync(draftContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditDraftContactPersonDto input)
        {
            var draftContactPerson = await _draftContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, draftContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftContactPersons_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}