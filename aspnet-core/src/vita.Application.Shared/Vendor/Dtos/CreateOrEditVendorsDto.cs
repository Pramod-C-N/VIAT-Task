using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using vita.Customer.Dtos;
using System.Collections.Generic;

namespace vita.Vendor.Dtos
{
    public class CreateOrEditVendorsDto : EntityDto<long?>
    {

        public string TenantType { get; set; }

        public string ConstitutionType { get; set; }

        public string Name { get; set; }

        public string LegalName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNumber { get; set; }

        public string EmailID { get; set; }

        public string Nationality { get; set; }

        public string Designation { get; set; }

        public CreateOrEditVendorAddressDto Address { get; set; }

        public List<CreateOrEditVendorDocumentsDto> Documents { get; set; }


        public CreateOrEditVendorTaxDetailsDto Taxdetails { get; set; }


        public CreateOrEditVendorForeignEntityDto foriegn { get; set; }

    }
}