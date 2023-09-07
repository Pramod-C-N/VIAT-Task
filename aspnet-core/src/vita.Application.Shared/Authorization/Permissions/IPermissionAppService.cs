using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Authorization.Permissions.Dto;

namespace vita.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
