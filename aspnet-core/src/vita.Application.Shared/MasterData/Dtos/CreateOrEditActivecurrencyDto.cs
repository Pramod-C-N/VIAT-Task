using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditActivecurrencyDto : EntityDto<int?>
    {

        public string Entity { get; set; }

        public string Currency { get; set; }

        public string AlphabeticCode { get; set; }

        public string NumericCode { get; set; }

        public string MinorUnit { get; set; }

        public bool IsActive { get; set; }

    }
}