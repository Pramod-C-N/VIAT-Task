using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using vita.Storage;

namespace vita.Web.Controllers
{
    public class VendorFileUpload : VendorFileUploadBase
    {
        public VendorFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
           : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
