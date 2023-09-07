using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerForeignEntitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerForeignEntityForViewDto>> GetAll(GetAllCustomerForeignEntitiesInput input);

        Task<GetCustomerForeignEntityForEditOutput> GetCustomerForeignEntityForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerForeignEntityDto input);

        Task Delete(EntityDto<long> input);

    }
}