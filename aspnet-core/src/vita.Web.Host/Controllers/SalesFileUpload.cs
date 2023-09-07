using Abp.AspNetCore.Mvc.Authorization;
using vita.Authorization;
using vita.Storage;
using Abp.BackgroundJobs;

namespace vita.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class SalesFileUpload : SalesFileUploadBase
    {
        public SalesFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}