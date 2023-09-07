using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Vendor.Dtos
{
    public class CreateOrEditVendorOwnershipDetailsDto : EntityDto<long?>
    {

        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string PartnerName { get; set; }

        public string PartnerConstitution { get; set; }

        public string PartnerNationality { get; set; }

        public decimal CapitalAmount { get; set; }

        public decimal CapitalShare { get; set; }

        public decimal ProfitShare { get; set; }

        public string RepresentativeName { get; set; }

    }
}