using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IHeadOfPaymentAppService : IApplicationService
    {
        Task<PagedResultDto<GetHeadOfPaymentForViewDto>> GetAll(GetAllHeadOfPaymentInput input);

        Task<GetHeadOfPaymentForViewDto> GetHeadOfPaymentForView(int id);

        Task<GetHeadOfPaymentForEditOutput> GetHeadOfPaymentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditHeadOfPaymentDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetHeadOfPaymentToExcel(GetAllHeadOfPaymentForExcelInput input);

    }
}