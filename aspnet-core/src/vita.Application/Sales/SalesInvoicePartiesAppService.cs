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
    [AbpAuthorize(AppPermissions.Pages_SalesInvoiceParties)]
    public class SalesInvoicePartiesAppService : vitaAppServiceBase, ISalesInvoicePartiesAppService
    {
        private readonly IRepository<SalesInvoiceParty, long> _salesInvoicePartyRepository;

        public SalesInvoicePartiesAppService(IRepository<SalesInvoiceParty, long> salesInvoicePartyRepository)
        {
            _salesInvoicePartyRepository = salesInvoicePartyRepository;

        }

        public async Task<PagedResultDto<GetSalesInvoicePartyForViewDto>> GetAll(GetAllSalesInvoicePartiesInput input)
        {

            var filteredSalesInvoiceParties = _salesInvoicePartyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.RegistrationName.Contains(input.Filter) || e.VATID.Contains(input.Filter) || e.GroupVATID.Contains(input.Filter) || e.CRNumber.Contains(input.Filter) || e.OtherID.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Type.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationNameFilter), e => e.RegistrationName.Contains(input.RegistrationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATIDFilter), e => e.VATID.Contains(input.VATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupVATIDFilter), e => e.GroupVATID.Contains(input.GroupVATIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CRNumberFilter), e => e.CRNumber.Contains(input.CRNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherIDFilter), e => e.OtherID.Contains(input.OtherIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter));

            var pagedAndFilteredSalesInvoiceParties = filteredSalesInvoiceParties
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoiceParties = from o in pagedAndFilteredSalesInvoiceParties
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

            var totalCount = await filteredSalesInvoiceParties.CountAsync();

            var dbList = await salesInvoiceParties.ToListAsync();
            var results = new List<GetSalesInvoicePartyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoicePartyForViewDto()
                {
                    SalesInvoiceParty = new SalesInvoicePartyDto
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

            return new PagedResultDto<GetSalesInvoicePartyForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoicePartyForViewDto> GetSalesInvoicePartyForView(long id)
        {
            var salesInvoiceParty = await _salesInvoicePartyRepository.GetAsync(id);

            var output = new GetSalesInvoicePartyForViewDto { SalesInvoiceParty = ObjectMapper.Map<SalesInvoicePartyDto>(salesInvoiceParty) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceParties_Edit)]
        public async Task<GetSalesInvoicePartyForEditOutput> GetSalesInvoicePartyForEdit(EntityDto<long> input)
        {
            var salesInvoiceParty = await _salesInvoicePartyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoicePartyForEditOutput { SalesInvoiceParty = ObjectMapper.Map<CreateOrEditSalesInvoicePartyDto>(salesInvoiceParty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoicePartyDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceParties_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoicePartyDto input)
        {
            var salesInvoiceParty = ObjectMapper.Map<SalesInvoiceParty>(input);
            salesInvoiceParty.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                salesInvoiceParty.TenantId = (int?)AbpSession.TenantId;
            }

            await _salesInvoicePartyRepository.InsertAsync(salesInvoiceParty);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceParties_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoicePartyDto input)
        {
            var salesInvoiceParty = await _salesInvoicePartyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoiceParty);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoiceParties_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoicePartyRepository.DeleteAsync(input.Id);
        }

    }
}