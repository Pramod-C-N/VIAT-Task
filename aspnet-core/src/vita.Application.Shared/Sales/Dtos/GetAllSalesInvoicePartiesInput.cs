using Abp.Application.Services.Dto;
using System;

namespace vita.Sales.Dtos
{
    public class GetAllSalesInvoicePartiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string RegistrationNameFilter { get; set; }

        public string VATIDFilter { get; set; }

        public string GroupVATIDFilter { get; set; }

        public string CRNumberFilter { get; set; }

        public string OtherIDFilter { get; set; }

        public string CustomerIdFilter { get; set; }

        public string TypeFilter { get; set; }

        public string AdditionalData1Filter { get; set; }

        public string LanguageFilter { get; set; }

        public string OtherDocumentTypeIdFilter { get; set; }

    }
}