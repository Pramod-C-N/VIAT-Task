using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorAddressesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string StreetFilter { get; set; }

        public string AdditionalStreetFilter { get; set; }

        public string BuildingNoFilter { get; set; }

        public string AdditionalNoFilter { get; set; }

        public string CityFilter { get; set; }

        public string PostalCodeFilter { get; set; }

        public string StateFilter { get; set; }

        public string NeighbourhoodFilter { get; set; }

        public string CountryCodeFilter { get; set; }

        public string TypeFilter { get; set; }

    }
}