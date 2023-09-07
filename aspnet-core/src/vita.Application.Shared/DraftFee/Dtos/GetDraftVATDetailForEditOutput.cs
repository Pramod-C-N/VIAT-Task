using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.DraftFee.Dtos
{
    public class GetDraftVATDetailForEditOutput
    {
        public CreateOrEditDraftVATDetailDto DraftVATDetail { get; set; }

    }
}