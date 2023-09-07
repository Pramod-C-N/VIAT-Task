using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorDocumentsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorDocumentsForViewDto>> GetAll(GetAllVendorDocumentsesInput input);

        Task<GetVendorDocumentsForEditOutput> GetVendorDocumentsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorDocumentsDto input);

        Task Delete(EntityDto<long> input);

    }
}