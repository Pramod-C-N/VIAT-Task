using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetCurrencyForEditOutput
    {
        public CreateOrEditCurrencyDto Currency { get; set; }

    }
}