using System.Threading.Tasks;
using vita.Sessions.Dto;

namespace vita.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
