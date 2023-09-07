using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerSectorDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerSectorDetailForViewDto>> GetAll(GetAllCustomerSectorDetailsInput input);

        Task<GetCustomerSectorDetailForEditOutput> GetCustomerSectorDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerSectorDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}