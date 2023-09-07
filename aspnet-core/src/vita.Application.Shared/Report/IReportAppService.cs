using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using vita.Dto;

namespace vita.Report
{
    public interface IReportAppService
    {
        Task<FileDto> GetWhtDetailedReportToExcel(DateTime fromDate, DateTime toDate, string fileName, string tenantName, string code = null, bool total = false);
    }
}
