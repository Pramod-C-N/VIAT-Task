using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Vendor.Dtos
{
    public class GetVendorTaxDetailsForEditOutput
    {
        public CreateOrEditVendorTaxDetailsDto VendorTaxDetails { get; set; }

    }
}