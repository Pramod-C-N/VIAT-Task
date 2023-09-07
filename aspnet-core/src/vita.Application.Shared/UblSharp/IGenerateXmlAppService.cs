using System;
using System.Collections.Generic;
using System.Text;
using vita.Sales.Dtos;
using UblSharp;
using Abp.Application.Services;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using System.Threading.Tasks;

namespace vita.UblSharp
{
    public interface IGenerateXmlAppService :IApplicationService
    {
        Task<bool> GenerateXmlRequest_Invoice(CreateOrEditSalesInvoiceDto input, string invoiceno, string uniqueIdentifier, string tenantId,string xml_uid="");
        Task<bool> GenerateXmlRequest_CreditNote(CreateOrEditCreditNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId, string xml_uid = "");
        Task<bool> GenerateXmlRequest_DebitNote(CreateOrEditDebitNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId, string xml_uid = "");
    }
}
