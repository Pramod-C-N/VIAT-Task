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
    [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteContactPerson)]
    public class PurchaseCreditNoteContactPersonAppService : vitaAppServiceBase, IPurchaseCreditNoteContactPersonAppService
    {
        private readonly IRepository<PurchaseCreditNoteContactPerson, long> _purchaseCreditNoteContactPersonRepository;

        public PurchaseCreditNoteContactPersonAppService(IRepository<PurchaseCreditNoteContactPerson, long> purchaseCreditNoteContactPersonRepository)
        {
            _purchaseCreditNoteContactPersonRepository = purchaseCreditNoteContactPersonRepository;

        }

        public async Task<PagedResultDto<GetPurchaseCreditNoteContactPersonForViewDto>> GetAll(GetAllPurchaseCreditNoteContactPersonInput input)
        {

            var filteredPurchaseCreditNoteContactPerson = _purchaseCreditNoteContactPersonRepository.GetAll()
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

            var pagedAndFilteredPurchaseCreditNoteContactPerson = filteredPurchaseCreditNoteContactPerson
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseCreditNoteContactPerson = from o in pagedAndFilteredPurchaseCreditNoteContactPerson
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

            var totalCount = await filteredPurchaseCreditNoteContactPerson.CountAsync();

            var dbList = await purchaseCreditNoteContactPerson.ToListAsync();
            var results = new List<GetPurchaseCreditNoteContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseCreditNoteContactPersonForViewDto()
                {
                    PurchaseCreditNoteContactPerson = new PurchaseCreditNoteContactPersonDto
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

            return new PagedResultDto<GetPurchaseCreditNoteContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPurchaseCreditNoteContactPersonForViewDto> GetPurchaseCreditNoteContactPersonForView(long id)
        {
            var purchaseCreditNoteContactPerson = await _purchaseCreditNoteContactPersonRepository.GetAsync(id);

            var output = new GetPurchaseCreditNoteContactPersonForViewDto { PurchaseCreditNoteContactPerson = ObjectMapper.Map<PurchaseCreditNoteContactPersonDto>(purchaseCreditNoteContactPerson) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Edit)]
        public async Task<GetPurchaseCreditNoteContactPersonForEditOutput> GetPurchaseCreditNoteContactPersonForEdit(EntityDto<long> input)
        {
            var purchaseCreditNoteContactPerson = await _purchaseCreditNoteContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseCreditNoteContactPersonForEditOutput { PurchaseCreditNoteContactPerson = ObjectMapper.Map<CreateOrEditPurchaseCreditNoteContactPersonDto>(purchaseCreditNoteContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseCreditNoteContactPersonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseCreditNoteContactPersonDto input)
        {
            var purchaseCreditNoteContactPerson = ObjectMapper.Map<PurchaseCreditNoteContactPerson>(input);
            purchaseCreditNoteContactPerson.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseCreditNoteContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseCreditNoteContactPersonRepository.InsertAsync(purchaseCreditNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseCreditNoteContactPersonDto input)
        {
            var purchaseCreditNoteContactPerson = await _purchaseCreditNoteContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseCreditNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseCreditNoteContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}