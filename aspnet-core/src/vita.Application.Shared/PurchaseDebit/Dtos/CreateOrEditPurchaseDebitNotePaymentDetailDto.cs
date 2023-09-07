using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseDebit.Dtos
{
    public class CreateOrEditPurchaseDebitNotePaymentDetailDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string PaymentMeans { get; set; }

        public string CreditDebitReasonText { get; set; }

        public string PaymentTerms { get; set; }

    }
}