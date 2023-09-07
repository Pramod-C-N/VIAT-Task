using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ITaxSubCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaxSubCategoryForViewDto>> GetAll(GetAllTaxSubCategoryInput input);

        Task<GetTaxSubCategoryForViewDto> GetTaxSubCategoryForView(int id);

        Task<GetTaxSubCategoryForEditOutput> GetTaxSubCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaxSubCategoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTaxSubCategoryToExcel(GetAllTaxSubCategoryForExcelInput input);

    }
}