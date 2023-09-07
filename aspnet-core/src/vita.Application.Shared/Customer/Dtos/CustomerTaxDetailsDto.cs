using System;
using Abp.Application.Services.Dto;

namespace vita.Customer.Dtos
{
    public class CustomerTaxDetailsDto : EntityDto<long>
    {
        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string BusinessCategory { get; set; }

        public string OperatingModel { get; set; }

        public string BusinessSupplies { get; set; }

        public string SalesVATCategory { get; set; }

        public string InvoiceType { get; set; }

    }
}