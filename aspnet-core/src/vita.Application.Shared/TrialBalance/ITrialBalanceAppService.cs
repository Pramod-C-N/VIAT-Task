using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Vendor.Dtos;
using vita.Dto;

namespace vita.TrialBalance
{
    public interface ITrialBalancesesAppService : IApplicationService
    {

        Task<bool> InsertBatchUploadTrailBalance(string json, string fileName, int? tenantId);



    }
}