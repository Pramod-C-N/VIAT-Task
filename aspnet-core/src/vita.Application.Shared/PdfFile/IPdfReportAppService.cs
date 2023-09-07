using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.Sales.Dtos;

namespace vita.PdfFile
{
    public interface IPdfReportAppService : IApplicationService
    {
        Task<InvoiceResponse> GetPDFFile_Invoice(CreateOrEditSalesInvoiceDto input, string invoiceno, string uniqueIdentifier, string tenantId);
        Task<InvoiceResponse> GetPDFFile_CreditNote(CreateOrEditCreditNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId);
        Task<InvoiceResponse> GetPDFFile_DebitNote(CreateOrEditDebitNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId);

    }
}
