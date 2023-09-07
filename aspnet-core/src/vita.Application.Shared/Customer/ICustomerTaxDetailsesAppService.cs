using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerTaxDetailsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerTaxDetailsForViewDto>> GetAll(GetAllCustomerTaxDetailsesInput input);

        Task<GetCustomerTaxDetailsForEditOutput> GetCustomerTaxDetailsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerTaxDetailsDto input);

        Task Delete(EntityDto<long> input);

    }
}