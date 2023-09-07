using System;
using Abp.Application.Services.Dto;

namespace vita.Vendor.Dtos
{
    public class VendorForeignEntityDto : EntityDto<long>
    {
        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string ForeignEntityName { get; set; }

        public string ForeignEntityAddress { get; set; }

        public string LegalRepresentative { get; set; }

        public string Country { get; set; }

    }
}