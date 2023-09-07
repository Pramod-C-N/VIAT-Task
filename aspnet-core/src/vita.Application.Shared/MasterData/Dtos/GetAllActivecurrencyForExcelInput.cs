using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllActivecurrencyForExcelInput
    {
        public string Filter { get; set; }

        public string EntityFilter { get; set; }

        public string CurrencyFilter { get; set; }

        public string AlphabeticCodeFilter { get; set; }

        public string NumericCodeFilter { get; set; }

        public string MinorUnitFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}