using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IPaymentMeansAppService : IApplicationService
    {
        Task<PagedResultDto<GetPaymentMeansForViewDto>> GetAll(GetAllPaymentMeansInput input);

        Task<GetPaymentMeansForViewDto> GetPaymentMeansForView(int id);

        Task<GetPaymentMeansForEditOutput> GetPaymentMeansForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPaymentMeansDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPaymentMeansToExcel(GetAllPaymentMeansForExcelInput input);

    }
}