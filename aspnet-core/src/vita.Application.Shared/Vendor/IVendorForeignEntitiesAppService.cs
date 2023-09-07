using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorForeignEntitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorForeignEntityForViewDto>> GetAll(GetAllVendorForeignEntitiesInput input);

        Task<GetVendorForeignEntityForEditOutput> GetVendorForeignEntityForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorForeignEntityDto input);

        Task Delete(EntityDto<long> input);

    }
}