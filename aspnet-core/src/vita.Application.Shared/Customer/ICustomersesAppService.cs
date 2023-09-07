using System;
using System.Data;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomersesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomersForViewDto>> GetAll(GetAllCustomersesInput input);

        Task<GetCustomersForEditOutput> GetCustomersForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomersDto input);

        Task Delete(EntityDto<long> input);
        Task<bool> CreateCustomer(CreateOrEditCustomersDto input);
        Task<bool> InsertBatchUploadCustomer(string json, string fileName, int? tenantId);

        Task<DataTable> GetMasterInvalidRecord(int batchid);


    }
}