using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorForeignEntitiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string ForeignEntityNameFilter { get; set; }

        public string ForeignEntityAddressFilter { get; set; }

        public string LegalRepresentativeFilter { get; set; }

        public string CountryFilter { get; set; }

    }
}