using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Vendor.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Vendor
{
    [AbpAuthorize(AppPermissions.Pages_VendorContactPersons)]
    public class VendorContactPersonsAppService : vitaAppServiceBase, IVendorContactPersonsAppService
    {
        private readonly IRepository<VendorContactPerson, long> _vendorContactPersonRepository;

        public VendorContactPersonsAppService(IRepository<VendorContactPerson, long> vendorContactPersonRepository)
        {
            _vendorContactPersonRepository = vendorContactPersonRepository;

        }

        public async Task<PagedResultDto<GetVendorContactPersonForViewDto>> GetAll(GetAllVendorContactPersonsInput input)
        {

            var filteredVendorContactPersons = _vendorContactPersonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.EmployeeCode.Contains(input.Filter) || e.ContactNumber.Contains(input.Filter) || e.GovtId.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeCodeFilter), e => e.EmployeeCode.Contains(input.EmployeeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNumberFilter), e => e.ContactNumber.Contains(input.ContactNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovtIdFilter), e => e.GovtId.Contains(input.GovtIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredVendorContactPersons = filteredVendorContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorContactPersons = from o in pagedAndFilteredVendorContactPersons
                                       select new
                                       {

                                           o.VendorID,
                                           o.VendorUniqueIdentifier,
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

            var totalCount = await filteredVendorContactPersons.CountAsync();

            var dbList = await vendorContactPersons.ToListAsync();
            var results = new List<GetVendorContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorContactPersonForViewDto()
                {
                    VendorContactPerson = new VendorContactPersonDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
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

            return new PagedResultDto<GetVendorContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorContactPersons_Edit)]
        public async Task<GetVendorContactPersonForEditOutput> GetVendorContactPersonForEdit(EntityDto<long> input)
        {
            var vendorContactPerson = await _vendorContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorContactPersonForEditOutput { VendorContactPerson = ObjectMapper.Map<CreateOrEditVendorContactPersonDto>(vendorContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorContactPersonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditVendorContactPersonDto input)
        {
            var vendorContactPerson = ObjectMapper.Map<VendorContactPerson>(input);

            if (AbpSession.TenantId != null)
            {
                vendorContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorContactPersonRepository.InsertAsync(vendorContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditVendorContactPersonDto input)
        {
            var vendorContactPerson = await _vendorContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorContactPersons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}