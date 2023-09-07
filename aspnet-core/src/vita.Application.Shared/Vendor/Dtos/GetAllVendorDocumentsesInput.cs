using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorDocumentsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string DocumentTypeCodeFilter { get; set; }

        public string DocumentNameFilter { get; set; }

        public string DocumentNumberFilter { get; set; }

        public DateTime? MaxDoumentDateFilter { get; set; }
        public DateTime? MinDoumentDateFilter { get; set; }

        public string StatusFilter { get; set; }

    }
}