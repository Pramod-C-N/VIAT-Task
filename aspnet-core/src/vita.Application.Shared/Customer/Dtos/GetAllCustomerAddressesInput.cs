using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerAddressesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

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