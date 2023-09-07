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
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteContactPerson)]
    public class PurchaseDebitNoteContactPersonAppService : vitaAppServiceBase, IPurchaseDebitNoteContactPersonAppService
    {
        private readonly IRepository<PurchaseDebitNoteContactPerson, long> _purchaseDebitNoteContactPersonRepository;

        public PurchaseDebitNoteContactPersonAppService(IRepository<PurchaseDebitNoteContactPerson, long> purchaseDebitNoteContactPersonRepository)
        {
            _purchaseDebitNoteContactPersonRepository = purchaseDebitNoteContactPersonRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNoteContactPersonForViewDto>> GetAll(GetAllPurchaseDebitNoteContactPersonInput input)
        {

            var filteredPurchaseDebitNoteContactPerson = _purchaseDebitNoteContactPersonRepository.GetAll()
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

            var pagedAndFilteredPurchaseDebitNoteContactPerson = filteredPurchaseDebitNoteContactPerson
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteContactPerson = from o in pagedAndFilteredPurchaseDebitNoteContactPerson
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

            var totalCount = await filteredPurchaseDebitNoteContactPerson.CountAsync();

            var dbList = await purchaseDebitNoteContactPerson.ToListAsync();
            var results = new List<GetPurchaseDebitNoteContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNoteContactPersonForViewDto()
                {
                    PurchaseDebitNoteContactPerson = new PurchaseDebitNoteContactPersonDto
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

            return new PagedResultDto<GetPurchaseDebitNoteContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Edit)]
        public async Task<GetPurchaseDebitNoteContactPersonForEditOutput> GetPurchaseDebitNoteContactPersonForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteContactPerson = await _purchaseDebitNoteContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNoteContactPersonForEditOutput { PurchaseDebitNoteContactPerson = ObjectMapper.Map<CreateOrEditPurchaseDebitNoteContactPersonDto>(purchaseDebitNoteContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNoteContactPersonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNoteContactPersonDto input)
        {
            var purchaseDebitNoteContactPerson = ObjectMapper.Map<PurchaseDebitNoteContactPerson>(input);
            purchaseDebitNoteContactPerson.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNoteContactPersonRepository.InsertAsync(purchaseDebitNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNoteContactPersonDto input)
        {
            var purchaseDebitNoteContactPerson = await _purchaseDebitNoteContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNoteContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}