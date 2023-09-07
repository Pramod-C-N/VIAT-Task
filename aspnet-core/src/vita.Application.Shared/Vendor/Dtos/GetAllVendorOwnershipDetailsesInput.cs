using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorOwnershipDetailsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VendorIDFilter { get; set; }

        public Guid? VendorUniqueIdentifierFilter { get; set; }

        public string PartnerNameFilter { get; set; }

        public string PartnerConstitutionFilter { get; set; }

        public string PartnerNationalityFilter { get; set; }

        public decimal? MaxCapitalAmountFilter { get; set; }
        public decimal? MinCapitalAmountFilter { get; set; }

        public decimal? MaxCapitalShareFilter { get; set; }
        public decimal? MinCapitalShareFilter { get; set; }

        public decimal? MaxProfitShareFilter { get; set; }
        public decimal? MinProfitShareFilter { get; set; }

        public string RepresentativeNameFilter { get; set; }

    }
}