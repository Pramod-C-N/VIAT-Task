using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using vita.Storage;

namespace vita.Web.Controllers
{
    public class DebitNotePurchaseFileUpload : DebitNotePurchaseFileUploadBase
    {
        public DebitNotePurchaseFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
           : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
