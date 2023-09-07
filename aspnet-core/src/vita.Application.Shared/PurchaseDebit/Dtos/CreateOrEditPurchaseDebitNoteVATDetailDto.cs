﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseDebit.Dtos
{
    public class CreateOrEditPurchaseDebitNoteVATDetailDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string TaxSchemeId { get; set; }

        public string VATCode { get; set; }

        public decimal VATRate { get; set; }

        public string ExcemptionReasonCode { get; set; }

        public string ExcemptionReasonText { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public string CurrencyCode { get; set; }

    }
}