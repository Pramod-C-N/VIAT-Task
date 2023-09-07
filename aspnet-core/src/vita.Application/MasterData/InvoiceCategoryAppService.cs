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
    [AbpAuthorize(AppPermissions.Pages_InvoiceCategory)]
    public class InvoiceCategoryAppService : vitaAppServiceBase, IInvoiceCategoryAppService
    {
        private readonly IRepository<InvoiceCategory> _invoiceCategoryRepository;
        private readonly IInvoiceCategoryExcelExporter _invoiceCategoryExcelExporter;

        public InvoiceCategoryAppService(IRepository<InvoiceCategory> invoiceCategoryRepository, IInvoiceCategoryExcelExporter invoiceCategoryExcelExporter)
        {
            _invoiceCategoryRepository = invoiceCategoryRepository;
            _invoiceCategoryExcelExporter = invoiceCategoryExcelExporter;

        }

        public async Task<PagedResultDto<GetInvoiceCategoryForViewDto>> GetAll(GetAllInvoiceCategoryInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredInvoiceCategory = _invoiceCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredInvoiceCategory = filteredInvoiceCategory
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var invoiceCategory = from o in pagedAndFilteredInvoiceCategory
                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          o.Code,
                                          o.IsActive,
                                          Id = o.Id
                                      };

                var totalCount = await filteredInvoiceCategory.CountAsync();

                var dbList = await invoiceCategory.ToListAsync();
                var results = new List<GetInvoiceCategoryForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetInvoiceCategoryForViewDto()
                    {
                        InvoiceCategory = new InvoiceCategoryDto
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

                return new PagedResultDto<GetInvoiceCategoryForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public async Task<GetInvoiceCategoryForViewDto> GetInvoiceCategoryForView(int id)
        {
            var invoiceCategory = await _invoiceCategoryRepository.GetAsync(id);

            var output = new GetInvoiceCategoryForViewDto { InvoiceCategory = ObjectMapper.Map<InvoiceCategoryDto>(invoiceCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceCategory_Edit)]
        public async Task<GetInvoiceCategoryForEditOutput> GetInvoiceCategoryForEdit(EntityDto input)
        {
            var invoiceCategory = await _invoiceCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetInvoiceCategoryForEditOutput { InvoiceCategory = ObjectMapper.Map<CreateOrEditInvoiceCategoryDto>(invoiceCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditInvoiceCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_InvoiceCategory_Create)]
        protected virtual async Task Create(CreateOrEditInvoiceCategoryDto input)
        {
            var invoiceCategory = ObjectMapper.Map<InvoiceCategory>(input);
            invoiceCategory.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                invoiceCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _invoiceCategoryRepository.InsertAsync(invoiceCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceCategory_Edit)]
        protected virtual async Task Update(CreateOrEditInvoiceCategoryDto input)
        {
            var invoiceCategory = await _invoiceCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, invoiceCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceCategory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _invoiceCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetInvoiceCategoryToExcel(GetAllInvoiceCategoryForExcelInput input)
        {

            var filteredInvoiceCategory = _invoiceCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredInvoiceCategory
                         select new GetInvoiceCategoryForViewDto()
                         {
                             InvoiceCategory = new InvoiceCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var invoiceCategoryListDtos = await query.ToListAsync();

            return _invoiceCategoryExcelExporter.ExportToFile(invoiceCategoryListDtos);
        }

    }
}