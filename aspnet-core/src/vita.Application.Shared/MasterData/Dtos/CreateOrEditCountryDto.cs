using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string StateName { get; set; }

        public string Sovereignty { get; set; }

        [RegularExpression(CountryConsts.AlphaCodeRegex)]
        [StringLength(CountryConsts.MaxAlphaCodeLength, MinimumLength = CountryConsts.MinAlphaCodeLength)]
        public string AlphaCode { get; set; }

        [RegularExpression(CountryConsts.NumericCodeRegex)]
        public string NumericCode { get; set; }

        public string InternetCCTLD { get; set; }

        public string SubDivisionCode { get; set; }

        [RegularExpression(CountryConsts.Alpha3CodeRegex)]
        [StringLength(CountryConsts.MaxAlpha3CodeLength, MinimumLength = CountryConsts.MinAlpha3CodeLength)]
        public string Alpha3Code { get; set; }

        public string CountryGroup { get; set; }

        public bool IsActive { get; set; }

    }
}