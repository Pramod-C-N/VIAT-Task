using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using vita.Storage;

namespace vita.Web.Controllers
{
    public class CreditNoteFileUpload : CreditNoteFileUploadBase
    {
        public CreditNoteFileUpload(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
