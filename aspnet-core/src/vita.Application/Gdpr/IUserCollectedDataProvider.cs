using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using vita.Dto;

namespace vita.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
