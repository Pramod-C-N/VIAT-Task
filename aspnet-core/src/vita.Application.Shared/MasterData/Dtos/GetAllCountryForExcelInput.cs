using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllCountryForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string SovereigntyFilter { get; set; }

        public string AlphaCodeFilter { get; set; }

        public string NumericCodeFilter { get; set; }

        public string InternetCCTLDFilter { get; set; }

        public string SubDivisionCodeFilter { get; set; }

        public string Alpha3CodeFilter { get; set; }

        public string CountryGroupFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}