using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IPurchaseTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseTypeForViewDto>> GetAll(GetAllPurchaseTypeInput input);

        Task<GetPurchaseTypeForViewDto> GetPurchaseTypeForView(int id);

        Task<GetPurchaseTypeForEditOutput> GetPurchaseTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPurchaseTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPurchaseTypeToExcel(GetAllPurchaseTypeForExcelInput input);

    }
}