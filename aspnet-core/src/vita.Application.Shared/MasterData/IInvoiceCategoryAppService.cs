using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IInvoiceCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetInvoiceCategoryForViewDto>> GetAll(GetAllInvoiceCategoryInput input);

        Task<GetInvoiceCategoryForViewDto> GetInvoiceCategoryForView(int id);

        Task<GetInvoiceCategoryForEditOutput> GetInvoiceCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditInvoiceCategoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetInvoiceCategoryToExcel(GetAllInvoiceCategoryForExcelInput input);

    }
}