using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ITaxCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaxCategoryForViewDto>> GetAll(GetAllTaxCategoryInput input);

        Task<GetTaxCategoryForViewDto> GetTaxCategoryForView(int id);

        Task<GetTaxCategoryForEditOutput> GetTaxCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaxCategoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTaxCategoryToExcel(GetAllTaxCategoryForExcelInput input);

    }
}