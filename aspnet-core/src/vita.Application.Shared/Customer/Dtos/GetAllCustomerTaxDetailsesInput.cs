using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerTaxDetailsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

        public string BusinessCategoryFilter { get; set; }

        public string OperatingModelFilter { get; set; }

        public string BusinessSuppliesFilter { get; set; }

        public string SalesVATCategoryFilter { get; set; }

        public string InvoiceTypeFilter { get; set; }

    }
}