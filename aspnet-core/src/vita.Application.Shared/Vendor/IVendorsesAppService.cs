using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.Vendor
{
    public interface IVendorsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVendorsForViewDto>> GetAll(GetAllVendorsesInput input);

        Task<GetVendorsForEditOutput> GetVendorsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditVendorsDto input);

        Task Delete(EntityDto<long> input);
        Task<bool> InsertBatchUploadVendor(string json, string fileName, int? tenantId);

        Task<bool> InsertBatchUploadLedger(string json, string fileName, int? tenantId);



    }
}