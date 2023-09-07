﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Credit.Dtos
{
    public class CreateOrEditCreditNoteItemDto : EntityDto<long?>
    {


        public string IRNNo { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BuyerIdentifier { get; set; }

        public string SellerIdentifier { get; set; }

        public string StandardIdentifier { get; set; }

        public decimal Quantity { get; set; }

        public string UOM { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public double DiscountAmount { get; set; }

        public decimal GrossPrice { get; set; }

        public decimal NetPrice { get; set; }

        public decimal VATRate { get; set; }

        public string VATCode { get; set; }

        public decimal VATAmount { get; set; }

        public decimal LineAmountInclusiveVAT { get; set; }

        public string CurrencyCode { get; set; }

        public string TaxSchemeId { get; set; }

        public string Notes { get; set; }

        public string ExcemptionReasonCode { get; set; }

        public string ExcemptionReasonText { get; set; }

        public string Language { get; set; }
        public string AdditionalData1 { get; set; }
        public string AdditionalData2 { get; set; }

        public bool isOtherCharges { get; set; }


    }
}