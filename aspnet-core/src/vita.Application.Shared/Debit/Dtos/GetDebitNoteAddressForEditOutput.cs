using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Debit.Dtos
{
    public class GetDebitNoteAddressForEditOutput
    {
        public CreateOrEditDebitNoteAddressDto DebitNoteAddress { get; set; }

    }
}