using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Purchase.Dtos
{
    public class CreateOrEditPurchaseEntryPaymentDetailDto : EntityDto<long?>
    {

        public Guid UniqueIdentifier { get; set; }

        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}