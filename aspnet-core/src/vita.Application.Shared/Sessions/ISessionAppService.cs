using System.Threading.Tasks;
using Abp.Application.Services;
using vita.Sessions.Dto;

namespace vita.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
