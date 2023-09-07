using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using vita.Storage;

namespace vita.Web.Controllers
{
    public class DebitNoteSalesFileUpload : DebitNoteSalesFileUploadBase
    {
        public DebitNoteSalesFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
           : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
