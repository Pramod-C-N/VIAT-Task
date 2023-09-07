using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetTaxCategoryForEditOutput
    {
        public CreateOrEditTaxCategoryDto TaxCategory { get; set; }

    }
}