using Abp.UI;
using AutoMapper;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Diagram;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Twilio.TwiML.Messaging;
using UblSharp;
using vita.Credit;
using vita.Credit.Dtos;
using vita.Debit;
using vita.Debit.Dtos;
using vita.DraftFee;
using vita.DraftFee.Dtos;
using vita.EInvoicing.Dto;
using vita.Filters;
using vita.Purchase;
using vita.Purchase.Dtos;
using vita.Sales;
using vita.Sales.Dtos;
using vita.UblSharp.Dtos;
using static vita.Filters.VitaFilter_Validation;

namespace vita.EInvoicing
{

    [VitaFilter_Authorization(VitaFilter_Authorization.VitaFilter_ModuleName.Einvoice)]
    public class Einvoice : vitaAppServiceBase, IEInvoicing
    {
        private readonly IMapper mapper;
        private readonly ICreditNoteAppService creditNoteAppService;
        private readonly IDebitNotesAppService debitNotesAppService;
        private readonly IPurchaseEntriesAppService purchaseEntriesAppService;
        private readonly ISalesInvoicesAppService salesInvoicesAppService;
        private readonly IDraftsAppService draftsAppService;

        public Einvoice(ISalesInvoicesAppService salesInvoicesAppService, IMapper mapper,
            ICreditNoteAppService creditNoteAppService,
            IDebitNotesAppService debitNotesAppService,
            IPurchaseEntriesAppService purchaseEntriesAppService,
            IDraftsAppService draftsAppService)
        {
            this.salesInvoicesAppService = salesInvoicesAppService;
            this.mapper = mapper;
            this.creditNoteAppService = creditNoteAppService;
            this.debitNotesAppService = debitNotesAppService;
            this.purchaseEntriesAppService = purchaseEntriesAppService;
            this.draftsAppService = draftsAppService;
        }

        [VitaFilter_Validation(VitaFilter_ValidationType.EinvoiceValidation)]
        public async Task<InvoiceResponse> SalesInvoice(InvoiceRequest request)
        {
                var req = mapper.Map<CreateOrEditSalesInvoiceDto>(request);
                var result = await salesInvoicesAppService.CreateSalesInvoice(req);  
            return result;
        }

        [VitaFilter_Validation(VitaFilter_ValidationType.EinvoiceValidation)]
        public async Task<InvoiceResponse> CreditNote(InvoiceRequest request)
        {
            var req = mapper.Map<CreateOrEditCreditNoteDto>(request);
            var result = await creditNoteAppService.CreateCreditNote(req);
            return result;
        }

        [VitaFilter_Validation(VitaFilter_ValidationType.EinvoiceValidation)]
        public async Task<InvoiceResponse> DebitNote(InvoiceRequest request)
        {
            var req = mapper.Map<CreateOrEditDebitNoteDto>(request);
            var result = await debitNotesAppService.CreateDebitNote(req);
            return result;
        }
        //public async Task<InvoiceResponse> PurchaseEntry(InvoiceRequest request)
        //{
        //    var req = mapper.Map<CreateOrEditPurchaseEntryDto>(request);
        //    var result = await purchaseEntriesAppService.CreatePurchaseEntry(req);
        //    return result;
        //}

        [VitaFilter_Validation(VitaFilter_ValidationType.UnicoreValidation)]
        public async Task<InvoiceResponse> Invoice(InvoiceRequestLanguage requestL)
        {
            var request = mapper.Map<InvoiceRequest>(requestL);

            InvoiceResponse result = new();

            if (requestL.GenerateDraft)
            {
                var req = mapper.Map<CreateOrEditDraftDto>(request);
                req.Source = "API";
                result = await draftsAppService.CreateDraft(req);
            }
            else if (request.InvoiceTypeCode == "383")
            {
                var req = mapper.Map<CreateOrEditDebitNoteDto>(request);
                result = await debitNotesAppService.CreateDebitNote(req);
            }
            else if (request.InvoiceTypeCode == "381")
            {
                var req = mapper.Map<CreateOrEditCreditNoteDto>(request);
                result = await creditNoteAppService.CreateCreditNote(req);
            }
            else if (request.InvoiceTypeCode == "388")
            {
                    var req = mapper.Map<CreateOrEditSalesInvoiceDto>(request);
                    result = await salesInvoicesAppService.CreateSalesInvoice(req);
            }
            return result;
        }


        [VitaFilter_Validation(VitaFilter_ValidationType.XmlValidation)]
        public async Task<InvoiceResponse> InvoiceXML(InvoiceXMLRequest request)
        {
            var input  = new InvoiceRequest();
            if (request.isParsed)
            {
                input = request.InvoiceRequest;
            }
            else
            {
                throw new UserFriendlyException("XML could not be parsed");
            }

            InvoiceResponse result = new();

            if (input.InvoiceType == InvoiceTypeEnum.Debit)
            {
                var req = mapper.Map<CreateOrEditDebitNoteDto>(input);
                result = await debitNotesAppService.CreateDebitNote(req);
            }
            if (input.InvoiceType == InvoiceTypeEnum.Credit)
            {
                var req = mapper.Map<CreateOrEditCreditNoteDto>(input);
                result = await creditNoteAppService.CreateCreditNote(req);
            }
            if (input.InvoiceType == InvoiceTypeEnum.Sales)
            {
                var req = mapper.Map<CreateOrEditSalesInvoiceDto>(input);
                result = await salesInvoicesAppService.CreateSalesInvoice(req);
            }
            return result;
        }

      





    }
}
