using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ITransactionCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetTransactionCategoryForViewDto>> GetAll(GetAllTransactionCategoryInput input);

        Task<GetTransactionCategoryForViewDto> GetTransactionCategoryForView(int id);

        Task<GetTransactionCategoryForEditOutput> GetTransactionCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTransactionCategoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTransactionCategoryToExcel(GetAllTransactionCategoryForExcelInput input);

    }
}