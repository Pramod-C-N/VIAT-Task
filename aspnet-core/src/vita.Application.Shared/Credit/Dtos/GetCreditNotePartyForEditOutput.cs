﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Credit.Dtos
{
    public class GetCreditNotePartyForEditOutput
    {
        public CreateOrEditCreditNotePartyDto CreditNoteParty { get; set; }

    }
}