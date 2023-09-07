using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class CountryDto : EntityDto
    {
        public string Name { get; set; }

        public string StateName { get; set; }

        public string Sovereignty { get; set; }

        public string AlphaCode { get; set; }

        public string NumericCode { get; set; }

        public string InternetCCTLD { get; set; }

        public string SubDivisionCode { get; set; }

        public string Alpha3Code { get; set; }

        public string CountryGroup { get; set; }

        public bool IsActive { get; set; }

    }
}