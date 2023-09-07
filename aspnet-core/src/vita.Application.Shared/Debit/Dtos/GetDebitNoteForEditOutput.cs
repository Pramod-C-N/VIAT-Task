using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Debit.Dtos
{
    public class GetDebitNoteForEditOutput
    {
        public CreateOrEditDebitNoteDto DebitNote { get; set; }

    }
}