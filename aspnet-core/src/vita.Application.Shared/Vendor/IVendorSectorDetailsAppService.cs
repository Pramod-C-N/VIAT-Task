using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorSectorDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorSectorDetailForViewDto>> GetAll(GetAllVendorSectorDetailsInput input);

        Task<GetVendorSectorDetailForEditOutput> GetVendorSectorDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorSectorDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}