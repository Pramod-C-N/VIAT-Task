using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorContactPersonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string NameFilter { get; set; }

        public string EmployeeCodeFilter { get; set; }

        public string ContactNumberFilter { get; set; }

        public string GovtIdFilter { get; set; }

        public string EmailFilter { get; set; }

        public string AddressFilter { get; set; }

        public string LocationFilter { get; set; }

        public string TypeFilter { get; set; }

    }
}