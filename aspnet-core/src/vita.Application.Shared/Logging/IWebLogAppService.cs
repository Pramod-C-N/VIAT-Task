using Abp.Application.Services;
using vita.Dto;
using vita.Logging.Dto;

namespace vita.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
