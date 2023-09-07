﻿using Abp.Application.Services.Dto;
using System;

namespace vita.Credit.Dtos
{
    public class GetAllCreditNotePaymentDetailInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string PaymentMeansFilter { get; set; }

        public string CreditDebitReasonTextFilter { get; set; }

        public string PaymentTermsFilter { get; set; }

    }
}