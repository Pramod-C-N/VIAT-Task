using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using vita.EInvoicing.Dto;
using vita.Sales.Dtos;

namespace vita.EInvoicing
{
    public interface IEInvoicing: IApplicationService
    {
        Task<InvoiceResponse> SalesInvoice(InvoiceRequest request);
        Task<InvoiceResponse> CreditNote(InvoiceRequest request);
        Task<InvoiceResponse> DebitNote(InvoiceRequest request);
        //Task<InvoiceResponse> PurchaseEntry(InvoiceRequest request);
        Task<InvoiceResponse> Invoice(InvoiceRequestLanguage requestL);
    }
}
