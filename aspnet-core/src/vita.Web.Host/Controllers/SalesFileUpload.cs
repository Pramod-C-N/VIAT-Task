using Abp.AspNetCore.Mvc.Authorization;
using vita.Authorization;
using vita.Storage;
using Abp.BackgroundJobs;
using vita.Sales;

namespace vita.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class SalesFileUpload : SalesFileUploadBase
    {
        public SalesFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager, ISalesInvoicesAppService sales
)
            : base(binaryObjectManager, backgroundJobManager,sales)
        {
        }
    }
}