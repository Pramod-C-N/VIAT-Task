using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.Sales
{
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceContactPersons)]
    public class SalesInvoiceContactPersonsAppService : vitaAppServiceBase, ISalesInvoiceContactPersonsAppService
    {
        private readonly IRepository<SalesInvoiceContactPerson, long> _salesInvoiceContactPersonRepository;

        public SalesInvoiceContactPersonsAppService(IRepository<SalesInvoiceContactPerson, long> salesInvoiceContactPersonRepository)
        {
            _salesInvoiceContactPersonRepository = salesInvoiceContactPersonRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoiceContactPersonForViewDto>> GetAll(GetAllSalesInvoiceContactPersonsInput input)
        {

            var filteredSalesInvoiceContactPersons = _salesInvoiceContactPersonRepository.GetAll()
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

            var pagedAndFilteredSalesInvoiceContactPersons = filteredSalesInvoiceContactPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceContactPersons = from o in pagedAndFilteredSalesInvoiceContactPersons
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

            var totalCount = await filteredSalesInvoiceContactPersons.CountAsync();

            var dbList = await salesInvoiceContactPersons.ToListAsync();
            var results = new List<GetSalesInvoiceContactPersonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceContactPersonForViewDto()
                {
                    SalesInvoiceContactPerson = new SalesInvoiceContactPersonDto
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

            return new PagedResultDto<GetSalesInvoiceContactPersonForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoiceContactPersonForViewDto> GetSalesInvoiceContactPersonForView(long id)
        {
            var salesInvoiceContactPerson = await _salesInvoiceContactPersonRepository.GetAsync(id);

            var output = new GetSalesInvoiceContactPersonForViewDto { SalesInvoiceContactPerson = ObjectMapper.Map<SalesInvoiceContactPersonDto>(salesInvoiceContactPerson) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceContactPersons_Edit)]
        public async Task<GetSalesInvoiceContactPersonForEditOutput> GetSalesInvoiceContactPersonForEdit(EntityDto<long> input)
        {
            var salesInvoiceContactPerson = await _salesInvoiceContactPersonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceContactPersonForEditOutput { SalesInvoiceContactPerson = ObjectMapper.Map<CreateOrEditSalesInvoiceContactPersonDto>(salesInvoiceContactPerson) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceContactPersonDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceContactPersons_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceContactPersonDto input)
        {
            var salesInvoiceContactPerson = ObjectMapper.Map<SalesInvoiceContactPerson>(input);
            salesInvoiceContactPerson.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                salesInvoiceContactPerson.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoiceContactPersonRepository.InsertAsync(salesInvoiceContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceContactPersons_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceContactPersonDto input)
        {
            var salesInvoiceContactPerson = await _salesInvoiceContactPersonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceContactPerson);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceContactPersons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceContactPersonRepository.DeleteAsync(input.Id);
        }

    }
}