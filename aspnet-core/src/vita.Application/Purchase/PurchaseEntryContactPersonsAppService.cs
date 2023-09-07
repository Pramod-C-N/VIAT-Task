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
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryContactPersons)]
    public class PurchaseEntryContactPersonsAppService : vitaAppServiceBase, IPurchaseEntryContactPersonsAppService
    {
        private readonly IRepository<PurchaseEntryContactPerson, long> _purchaseEntryContactPersonRepository;

        public PurchaseEntryContactPersonsAppService(IRepository<PurchaseEntryContactPerson, long> purchaseEntryContactPersonRepository)
        {
            _purchaseEntryContactPersonRepository = purchaseEntryContactPersonRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryContactPersonForViewDto>> GetAll(GetAllPurchaseEntryContactPersonsInput input)
        {

            var filteredPurchaseEntryContactPersons = _purchaseEntryContactPersonRepository.GetAll()
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

            var pagedAndFilteredPurchaseEntryContactPersons = filteredPurchaseEntryContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryContactPersons = from o in pagedAndFilteredPurchaseEntryContactPersons
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

            var totalCount = await filteredPurchaseEntryContactPersons.CountAsync();

            var dbList = await purchaseEntryContactPersons.ToListAsync();
            var results = new List<GetPurchaseEntryContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryContactPersonForViewDto()
                {
                    PurchaseEntryContactPerson = new PurchaseEntryContactPersonDto
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

            return new PagedResultDto<GetPurchaseEntryContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryContactPersons_Edit)]
        public async Task<GetPurchaseEntryContactPersonForEditOutput> GetPurchaseEntryContactPersonForEdit(EntityDto<long> input)
        {
            var purchaseEntryContactPerson = await _purchaseEntryContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryContactPersonForEditOutput { PurchaseEntryContactPerson = ObjectMapper.Map<CreateOrEditPurchaseEntryContactPersonDto>(purchaseEntryContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryContactPersonDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryContactPersonDto input)
        {
            var purchaseEntryContactPerson = ObjectMapper.Map<PurchaseEntryContactPerson>(input);
            purchaseEntryContactPerson.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryContactPersonRepository.InsertAsync(purchaseEntryContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryContactPersonDto input)
        {
            var purchaseEntryContactPerson = await _purchaseEntryContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryContactPersons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}