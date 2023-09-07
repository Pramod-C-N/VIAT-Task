using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IReasonCNDNAppService : IApplicationService
    {
        Task<PagedResultDto<GetReasonCNDNForViewDto>> GetAll(GetAllReasonCNDNInput input);

        Task<GetReasonCNDNForViewDto> GetReasonCNDNForView(int id);

        Task<GetReasonCNDNForEditOutput> GetReasonCNDNForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditReasonCNDNDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetReasonCNDNToExcel(GetAllReasonCNDNForExcelInput input);

    }
}