using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorContactPersonForViewDto>> GetAll(GetAllVendorContactPersonsInput input);

        Task<GetVendorContactPersonForEditOutput> GetVendorContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}