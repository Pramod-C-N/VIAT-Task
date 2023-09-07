using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using vita.Storage;

namespace vita.Web.Controllers
{
    public class LedgerFileUpload : LedgerFileUploadBase
    {
        public LedgerFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
           : base(binaryObjectManager, backgroundJobManager)
        {

        }
    }
}
