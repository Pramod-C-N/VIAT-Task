using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Exporting;
using vita.MasterData.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_InvoiceType)]
    public class InvoiceTypeAppService : vitaAppServiceBase, IInvoiceTypeAppService
    {
        private readonly IRepository<InvoiceType> _invoiceTypeRepository;
        private readonly IInvoiceTypeExcelExporter _invoiceTypeExcelExporter;

        public InvoiceTypeAppService(IRepository<InvoiceType> invoiceTypeRepository, IInvoiceTypeExcelExporter invoiceTypeExcelExporter)
        {
            _invoiceTypeRepository = invoiceTypeRepository;
            _invoiceTypeExcelExporter = invoiceTypeExcelExporter;

        }

        public async Task<PagedResultDto<GetInvoiceTypeForViewDto>> GetAll(GetAllInvoiceTypeInput input)
        {

            var filteredInvoiceType = _invoiceTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredInvoiceType = filteredInvoiceType
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var invoiceType = from o in pagedAndFilteredInvoiceType
                              select new
                              {

                                  o.Name,
                                  o.Description,
                                  o.Code,
                                  o.IsActive,
                                  Id = o.Id
                              };

            var totalCount = await filteredInvoiceType.CountAsync();

            var dbList = await invoiceType.ToListAsync();
            var results = new List<GetInvoiceTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetInvoiceTypeForViewDto()
                {
                    InvoiceType = new InvoiceTypeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Code = o.Code,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetInvoiceTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetInvoiceTypeForViewDto> GetInvoiceTypeForView(int id)
        {
            var invoiceType = await _invoiceTypeRepository.GetAsync(id);

            var output = new GetInvoiceTypeForViewDto { InvoiceType = ObjectMapper.Map<InvoiceTypeDto>(invoiceType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceType_Edit)]
        public async Task<GetInvoiceTypeForEditOutput> GetInvoiceTypeForEdit(EntityDto input)
        {
            var invoiceType = await _invoiceTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetInvoiceTypeForEditOutput { InvoiceType = ObjectMapper.Map<CreateOrEditInvoiceTypeDto>(invoiceType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditInvoiceTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_InvoiceType_Create)]
        protected virtual async Task Create(CreateOrEditInvoiceTypeDto input)
        {
            var invoiceType = ObjectMapper.Map<InvoiceType>(input);
            invoiceType.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                invoiceType.TenantId = (int?)AbpSession.TenantId;
            }

            await _invoiceTypeRepository.InsertAsync(invoiceType);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceType_Edit)]
        protected virtual async Task Update(CreateOrEditInvoiceTypeDto input)
        {
            var invoiceType = await _invoiceTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, invoiceType);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _invoiceTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetInvoiceTypeToExcel(GetAllInvoiceTypeForExcelInput input)
        {

            var filteredInvoiceType = _invoiceTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredInvoiceType
                         select new GetInvoiceTypeForViewDto()
                         {
                             InvoiceType = new InvoiceTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var invoiceTypeListDtos = await query.ToListAsync();

            return _invoiceTypeExcelExporter.ExportToFile(invoiceTypeListDtos);
        }

    }
}