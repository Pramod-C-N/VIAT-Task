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
    [AbpAuthorize(AppPermissions.Pages_VendorDocumentses)]
    public class VendorDocumentsesAppService : vitaAppServiceBase, IVendorDocumentsesAppService
    {
        private readonly IRepository<VendorDocuments, long> _vendorDocumentsRepository;

        public VendorDocumentsesAppService(IRepository<VendorDocuments, long> vendorDocumentsRepository)
        {
            _vendorDocumentsRepository = vendorDocumentsRepository;

        }

        public async Task<PagedResultDto<GetVendorDocumentsForViewDto>> GetAll(GetAllVendorDocumentsesInput input)
        {

            var filteredVendorDocumentses = _vendorDocumentsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VendorID.Contains(input.Filter) || e.DocumentTypeCode.Contains(input.Filter) || e.DocumentName.Contains(input.Filter) || e.DocumentNumber.Contains(input.Filter) || e.Status.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorIDFilter), e => e.VendorID.Contains(input.VendorIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VendorUniqueIdentifierFilter.ToString()), e => e.VendorUniqueIdentifier.ToString() == input.VendorUniqueIdentifierFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeCodeFilter), e => e.DocumentTypeCode.Contains(input.DocumentTypeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNameFilter), e => e.DocumentName.Contains(input.DocumentNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNumberFilter), e => e.DocumentNumber.Contains(input.DocumentNumberFilter))
                        .WhereIf(input.MinDoumentDateFilter != null, e => e.DoumentDate >= input.MinDoumentDateFilter)
                        .WhereIf(input.MaxDoumentDateFilter != null, e => e.DoumentDate <= input.MaxDoumentDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status.Contains(input.StatusFilter));

            var pagedAndFilteredVendorDocumentses = filteredVendorDocumentses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vendorDocumentses = from o in pagedAndFilteredVendorDocumentses
                                    select new
                                    {

                                        o.VendorID,
                                        o.VendorUniqueIdentifier,
                                        o.DocumentTypeCode,
                                        o.DocumentName,
                                        o.DocumentNumber,
                                        o.DoumentDate,
                                        o.Status,
                                        Id = o.Id
                                    };

            var totalCount = await filteredVendorDocumentses.CountAsync();

            var dbList = await vendorDocumentses.ToListAsync();
            var results = new List<GetVendorDocumentsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVendorDocumentsForViewDto()
                {
                    VendorDocuments = new VendorDocumentsDto
                    {

                        VendorID = o.VendorID,
                        VendorUniqueIdentifier = o.VendorUniqueIdentifier,
                        DocumentTypeCode = o.DocumentTypeCode,
                        DocumentName = o.DocumentName,
                        DocumentNumber = o.DocumentNumber,
                        DoumentDate = o.DoumentDate,
                        Status = o.Status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVendorDocumentsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_VendorDocumentses_Edit)]
        public async Task<GetVendorDocumentsForEditOutput> GetVendorDocumentsForEdit(EntityDto<long> input)
        {
            var vendorDocuments = await _vendorDocumentsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVendorDocumentsForEditOutput { VendorDocuments = ObjectMapper.Map<CreateOrEditVendorDocumentsDto>(vendorDocuments) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVendorDocumentsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_VendorDocumentses_Create)]
        protected virtual async Task Create(CreateOrEditVendorDocumentsDto input)
        {
            var vendorDocuments = ObjectMapper.Map<VendorDocuments>(input);

            if (AbpSession.TenantId != null)
            {
                vendorDocuments.TenantId = (int?)AbpSession.TenantId;
            }

            await _vendorDocumentsRepository.InsertAsync(vendorDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorDocumentses_Edit)]
        protected virtual async Task Update(CreateOrEditVendorDocumentsDto input)
        {
            var vendorDocuments = await _vendorDocumentsRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, vendorDocuments);

        }

        [AbpAuthorize(AppPermissions.Pages_VendorDocumentses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _vendorDocumentsRepository.DeleteAsync(input.Id);
        }

    }
}