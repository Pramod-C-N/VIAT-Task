using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerContactPersonForViewDto>> GetAll(GetAllCustomerContactPersonsInput input);

        Task<GetCustomerContactPersonForEditOutput> GetCustomerContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}