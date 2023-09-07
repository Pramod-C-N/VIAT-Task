using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantShareHoldersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PartnerNameFilter { get; set; }

        public string DesignationFilter { get; set; }

        public string NationalityFilter { get; set; }

        public string CapitalAmountFilter { get; set; }

        public string CapitalShareFilter { get; set; }

        public string ProfitShareFilter { get; set; }

        public string ConstitutionNameFilter { get; set; }

        public string RepresentativeNameFilter { get; set; }

        public string DomesticNameFilter { get; set; }

    }
}