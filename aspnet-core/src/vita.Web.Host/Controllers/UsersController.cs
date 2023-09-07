using Abp.AspNetCore.Mvc.Authorization;
using vita.Authorization;
using vita.Storage;
using Abp.BackgroundJobs;

namespace vita.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}