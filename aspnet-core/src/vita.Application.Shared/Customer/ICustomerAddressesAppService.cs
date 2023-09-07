using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerAddressForViewDto>> GetAll(GetAllCustomerAddressesInput input);

        Task<GetCustomerAddressForEditOutput> GetCustomerAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}