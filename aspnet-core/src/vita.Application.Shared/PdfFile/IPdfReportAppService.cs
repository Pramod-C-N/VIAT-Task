using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.EInvoicing.Dto;
using vita.Sales.Dtos;

namespace vita.PdfFile
{
    public interface IPdfReportAppService : IApplicationService
    {
        Task<InvoiceResponse> GeneratePdfRequest<T>(T inputGen, string invoiceno, string uniqueIdentifier, string tenantId, InvoiceTypeEnum invoiceType,bool isDraft=false);


    }
}
