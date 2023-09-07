using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditCurrencyDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string NumericCode { get; set; }

        public string MinorUnit { get; set; }

        public string Country { get; set; }

        public bool IsActive { get; set; }

    }
}