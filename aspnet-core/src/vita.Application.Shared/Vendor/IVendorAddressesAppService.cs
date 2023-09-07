using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorAddressForViewDto>> GetAll(GetAllVendorAddressesInput input);

        Task<GetVendorAddressForEditOutput> GetVendorAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}