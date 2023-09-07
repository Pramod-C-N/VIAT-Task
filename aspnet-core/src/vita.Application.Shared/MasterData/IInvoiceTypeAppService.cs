using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IInvoiceTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetInvoiceTypeForViewDto>> GetAll(GetAllInvoiceTypeInput input);

        Task<GetInvoiceTypeForViewDto> GetInvoiceTypeForView(int id);

        Task<GetInvoiceTypeForEditOutput> GetInvoiceTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditInvoiceTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetInvoiceTypeToExcel(GetAllInvoiceTypeForExcelInput input);

    }
}