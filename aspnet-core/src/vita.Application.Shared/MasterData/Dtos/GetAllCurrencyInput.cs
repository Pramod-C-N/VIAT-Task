using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllCurrencyInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string CodeFilter { get; set; }

        public string NumericCodeFilter { get; set; }

        public string MinorUnitFilter { get; set; }

        public string CountryFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}