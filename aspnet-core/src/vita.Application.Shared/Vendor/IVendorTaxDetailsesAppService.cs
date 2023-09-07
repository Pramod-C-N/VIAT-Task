using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorTaxDetailsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorTaxDetailsForViewDto>> GetAll(GetAllVendorTaxDetailsesInput input);

        Task<GetVendorTaxDetailsForEditOutput> GetVendorTaxDetailsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorTaxDetailsDto input);

        Task Delete(EntityDto<long> input);

    }
}