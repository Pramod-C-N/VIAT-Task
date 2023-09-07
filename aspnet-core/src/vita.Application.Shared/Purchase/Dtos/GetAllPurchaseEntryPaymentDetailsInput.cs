using Abp.Application.Services.Dto;
using System;

namespace vita.Purchase.Dtos
{
    public class GetAllPurchaseEntryPaymentDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public Guid? UniqueIdentifierFilter { get; set; }

        public string IRNNoFilter { get; set; }

        public string PaymentMeansFilter { get; set; }

        public string CreditDebitReasonTextFilter { get; set; }

        public string PaymentTermsFilter { get; set; }

    }
}