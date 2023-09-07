using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class ActivecurrencyDto : EntityDto
    {
        public string Entity { get; set; }

        public string Currency { get; set; }

        public string AlphabeticCode { get; set; }

        public string NumericCode { get; set; }

        public string MinorUnit { get; set; }

        public bool IsActive { get; set; }

    }
}