using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerOwnershipDetailsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerOwnershipDetailsForViewDto>> GetAll(GetAllCustomerOwnershipDetailsesInput input);

        Task<GetCustomerOwnershipDetailsForEditOutput> GetCustomerOwnershipDetailsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerOwnershipDetailsDto input);

        Task Delete(EntityDto<long> input);

    }
}