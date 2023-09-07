using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class CreateOrEditCustomerTaxDetailsDto : EntityDto<long?>
    {

        public Guid UniqueId { get; set; }

        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string BusinessCategory { get; set; }

        public string OperatingModel { get; set; }

        public string BusinessSupplies { get; set; }

        public string SalesVATCategory { get; set; }

        public string InvoiceType { get; set; }

    }
}