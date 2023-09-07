﻿using System;
using Abp.Application.Services.Dto;

namespace vita.PurchaseCredit.Dtos
{
    public class PurchaseCreditNoteDiscountDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public string VATCode { get; set; }

        public decimal VATRate { get; set; }

        public string TaxSchemeId { get; set; }

    }
}