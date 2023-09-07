using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorOwnershipDetailsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorOwnershipDetailsForViewDto>> GetAll(GetAllVendorOwnershipDetailsesInput input);

        Task<GetVendorOwnershipDetailsForEditOutput> GetVendorOwnershipDetailsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorOwnershipDetailsDto input);

        Task Delete(EntityDto<long> input);

    }
}