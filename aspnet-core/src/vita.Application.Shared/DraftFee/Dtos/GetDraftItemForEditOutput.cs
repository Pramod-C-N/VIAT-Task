using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.DraftFee.Dtos
{
    public class GetDraftItemForEditOutput
    {
        public CreateOrEditDraftItemDto DraftItem { get; set; }

    }
}