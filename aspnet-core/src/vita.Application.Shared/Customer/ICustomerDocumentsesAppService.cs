using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Customer.Dtos;
using vita.Dto;

namespace vita.Customer
{
    public interface ICustomerDocumentsesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerDocumentsForViewDto>> GetAll(GetAllCustomerDocumentsesInput input);

        Task<GetCustomerDocumentsForEditOutput> GetCustomerDocumentsForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerDocumentsDto input);

        Task Delete(EntityDto<long> input);

    }
}