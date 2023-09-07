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
    [AbpAuthorize(AppPermissions.Pages_DocumentMaster)]
    public class DocumentMasterAppService : vitaAppServiceBase, IDocumentMasterAppService
    {
        private readonly IRepository<DocumentMaster> _documentMasterRepository;
        private readonly IDocumentMasterExcelExporter _documentMasterExcelExporter;

        public DocumentMasterAppService(IRepository<DocumentMaster> documentMasterRepository, IDocumentMasterExcelExporter documentMasterExcelExporter)
        {
            _documentMasterRepository = documentMasterRepository;
            _documentMasterExcelExporter = documentMasterExcelExporter;

        }

        public async Task<PagedResultDto<GetDocumentMasterForViewDto>> GetAll(GetAllDocumentMasterInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredDocumentMaster = _documentMasterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Validformat.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValidformatFilter), e => e.Validformat.Contains(input.ValidformatFilter));

                var pagedAndFilteredDocumentMaster = filteredDocumentMaster
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var documentMaster = from o in pagedAndFilteredDocumentMaster
                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         o.Code,
                                         o.IsActive,
                                         o.Validformat,
                                         Id = o.Id
                                     };

                var totalCount = await filteredDocumentMaster.CountAsync();

                var dbList = await documentMaster.ToListAsync();
                var results = new List<GetDocumentMasterForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetDocumentMasterForViewDto()
                    {
                        DocumentMaster = new DocumentMasterDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            IsActive = o.IsActive,
                            Validformat = o.Validformat,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetDocumentMasterForViewDto>(
                    totalCount,
                    results
                );

            }

        }
        public async Task<GetDocumentMasterForViewDto> GetDocumentMasterForView(int id)
        {
            var documentMaster = await _documentMasterRepository.GetAsync(id);

            var output = new GetDocumentMasterForViewDto { DocumentMaster = ObjectMapper.Map<DocumentMasterDto>(documentMaster) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentMaster_Edit)]
        public async Task<GetDocumentMasterForEditOutput> GetDocumentMasterForEdit(EntityDto input)
        {
            var documentMaster = await _documentMasterRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDocumentMasterForEditOutput { DocumentMaster = ObjectMapper.Map<CreateOrEditDocumentMasterDto>(documentMaster) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDocumentMasterDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DocumentMaster_Create)]
        protected virtual async Task Create(CreateOrEditDocumentMasterDto input)
        {
            var documentMaster = ObjectMapper.Map<DocumentMaster>(input);
            documentMaster.UniqueIdentifier = new Guid();
            if (AbpSession.TenantId != null)
            {
                documentMaster.TenantId = (int?)AbpSession.TenantId;
            }

            await _documentMasterRepository.InsertAsync(documentMaster);

        }

        [AbpAuthorize(AppPermissions.Pages_DocumentMaster_Edit)]
        protected virtual async Task Update(CreateOrEditDocumentMasterDto input)
        {
            var documentMaster = await _documentMasterRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, documentMaster);

        }

        [AbpAuthorize(AppPermissions.Pages_DocumentMaster_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _documentMasterRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDocumentMasterToExcel(GetAllDocumentMasterForExcelInput input)
        {

            var filteredDocumentMaster = _documentMasterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Validformat.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValidformatFilter), e => e.Validformat.Contains(input.ValidformatFilter));

            var query = (from o in filteredDocumentMaster
                         select new GetDocumentMasterForViewDto()
                         {
                             DocumentMaster = new DocumentMasterDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Validformat = o.Validformat,
                                 Id = o.Id
                             }
                         });

            var documentMasterListDtos = await query.ToListAsync();

            return _documentMasterExcelExporter.ExportToFile(documentMasterListDtos);
        }

    }
}