﻿using Abp.Application.Services.Dto;
using System;

namespace vita.DraftFee.Dtos
{
    public class GetAllDraftPaymentDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string PaymentMeansFilter { get; set; }

        public string CreditDebitReasonTextFilter { get; set; }

        public string PaymentTermsFilter { get; set; }

        public string AdditionalData1Filter { get; set; }

        public string LanguageFilter { get; set; }

    }
}