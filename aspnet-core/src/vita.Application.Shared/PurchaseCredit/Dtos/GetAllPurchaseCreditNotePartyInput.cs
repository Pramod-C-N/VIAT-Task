using Abp.Application.Services.Dto;
using System;

namespace vita.PurchaseCredit.Dtos
{
    public class GetAllPurchaseCreditNotePartyInput : PagedAndSortedResultRequestDto
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

    }
}