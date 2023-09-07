using System;
using Abp.Application.Services.Dto;

namespace vita.Vendor.Dtos
{
    public class VendorSectorDetailDto : EntityDto<long>
    {
        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string SubIndustryCode { get; set; }

        public string SubIndustryName { get; set; }

    }
}