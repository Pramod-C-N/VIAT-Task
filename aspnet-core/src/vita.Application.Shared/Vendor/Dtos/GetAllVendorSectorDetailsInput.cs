using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorSectorDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string SubIndustryCodeFilter { get; set; }

        public string SubIndustryNameFilter { get; set; }

    }
}