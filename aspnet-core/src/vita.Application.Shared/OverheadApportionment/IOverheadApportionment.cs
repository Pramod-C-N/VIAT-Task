using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using vita.Organizations.Dto;
using vita.OverheadApportionment.Dto;

namespace vita.OverheadApportionment
{
    public interface IOverheadApportionment : IApplicationService
    {
        Task<DataTable> GetOverheadApportionmentPreviousData();

        Task<bool> CreateOverheadApportionmentPreviousData(OverheadApportionmentPreviousDataDTO input);
        Task<DataTable> GetOverheadApportionmentCurrentDataSummary();

        Task<bool> CreateOverheadApportionmentCurrentDataSummary(OverheadApportionmentPreviousDataDTO input);
        Task<DataTable> GetOverheadApportionmentCurrentDataDetailed(string type);

        Task<bool> CreateOverheadApportionmentCurrentDataDetailed(List<OverheadApportionmentCurrentDataDetailedDTO> input);
    }
}
