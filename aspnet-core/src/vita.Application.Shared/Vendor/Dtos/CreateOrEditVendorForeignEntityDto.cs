using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Vendor.Dtos
{
    public class CreateOrEditVendorForeignEntityDto : EntityDto<long?>
    {

        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string ForeignEntityName { get; set; }

        public string ForeignEntityAddress { get; set; }

        public string LegalRepresentative { get; set; }

        public string Country { get; set; }

    }
}