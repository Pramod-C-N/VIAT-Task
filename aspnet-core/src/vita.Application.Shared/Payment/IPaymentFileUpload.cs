using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace vita.Payment
{
    public interface IPaymentFileUpload : IApplicationService
    {
        Task<bool> InsertBatchUploadPayment(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate);
    }
}
