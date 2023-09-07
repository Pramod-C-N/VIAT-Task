using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseCredit.Dtos
{
    public class CreateOrEditPurchaseCreditNotePaymentDetailDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}