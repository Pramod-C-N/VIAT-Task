using System;
using Abp.Application.Services.Dto;

namespace vita.Vendor.Dtos
{
    public class VendorsDto : EntityDto<long>
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

    }
}