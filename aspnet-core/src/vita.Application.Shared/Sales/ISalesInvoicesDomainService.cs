using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;
using System.Data;
using System.Collections.Generic;
using Abp.Domain.Services;

namespace vita.Sales
{
    public interface ISalesInvoicesDomainService : IDomainService
    {

        Task<InvoiceResponse> GetSalesInvoiceData(int? irnNo,int tenantId,string transtype);
        Task<bool> UpdateInvoiceURL(InvoiceResponse response,int tenantId, string type);
        Task<InvoiceResponse> GenerateDraftInvoice(int? irnNo, int tenantId, string transType);

    }
}