using System.Threading.Tasks;
using Abp.Application.Services;
using vita.Editions.Dto;
using vita.MultiTenancy.Dto;

namespace vita.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}