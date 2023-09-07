using Abp.Application.Services.Dto;
using System;

namespace vita.Vendor.Dtos
{
    public class GetAllVendorsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TenantTypeFilter { get; set; }

        public string ConstitutionTypeFilter { get; set; }

        public string NameFilter { get; set; }

        public string LegalNameFilter { get; set; }

        public string ContactPersonFilter { get; set; }

        public string ContactNumberFilter { get; set; }

        public string EmailIDFilter { get; set; }

        public string NationalityFilter { get; set; }

        public string DesignationFilter { get; set; }

    }
}