using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.DraftFee.Dtos
{
    public class GetDraftContactPersonForEditOutput
    {
        public CreateOrEditDraftContactPersonDto DraftContactPerson { get; set; }

    }
}