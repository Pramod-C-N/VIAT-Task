using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IDocumentMasterAppService : IApplicationService
    {
        Task<PagedResultDto<GetDocumentMasterForViewDto>> GetAll(GetAllDocumentMasterInput input);

        Task<GetDocumentMasterForViewDto> GetDocumentMasterForView(int id);

        Task<GetDocumentMasterForEditOutput> GetDocumentMasterForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDocumentMasterDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDocumentMasterToExcel(GetAllDocumentMasterForExcelInput input);

    }
}