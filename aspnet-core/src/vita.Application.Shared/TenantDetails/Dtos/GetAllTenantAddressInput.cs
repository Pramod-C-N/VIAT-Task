using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantAddressInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string AddressTypeIdFilter { get; set; }

        public string AddressTypeFilter { get; set; }

        public string BuildingNoFilter { get; set; }

        public string AdditionalBuildingNumberFilter { get; set; }

        public string StreetFilter { get; set; }

        public string AdditionalStreetFilter { get; set; }

        public string NeighbourhoodFilter { get; set; }

        public string CityFilter { get; set; }

        public string StateFilter { get; set; }

        public string PostalCodeFilter { get; set; }

        public string CountryFilter { get; set; }

        public string CountryCodeFilter { get; set; }

    }
}