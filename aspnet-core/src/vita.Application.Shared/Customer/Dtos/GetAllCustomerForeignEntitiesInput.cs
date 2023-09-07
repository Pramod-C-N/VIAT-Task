using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerForeignEntitiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

        public string ForeignEntityNameFilter { get; set; }

        public string ForeignEntityAddressFilter { get; set; }

        public string LegalRepresentativeFilter { get; set; }

        public string CountryFilter { get; set; }

    }
}