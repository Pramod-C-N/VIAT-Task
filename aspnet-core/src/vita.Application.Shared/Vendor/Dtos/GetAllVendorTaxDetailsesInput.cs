using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorTaxDetailsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string BusinessCategoryFilter { get; set; }

        public string OperatingModelFilter { get; set; }

        public string BusinessSuppliesFilter { get; set; }

        public string SalesVATCategoryFilter { get; set; }

        public string InvoiceTypeFilter { get; set; }

    }
}