using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Vendor.Dtos
{
    public class CreateOrEditVendorTaxDetailsDto : EntityDto<long?>
    {

        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string BusinessCategory { get; set; }

        public string OperatingModel { get; set; }

        public string BusinessSupplies { get; set; }

        public string SalesVATCategory { get; set; }

        public string InvoiceType { get; set; }

    }
}