using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantBasicDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TenantTypeFilter { get; set; }

        public string ConstitutionTypeFilter { get; set; }

        public string BusinessCategoryFilter { get; set; }

        public string OperationalModelFilter { get; set; }

        public string TurnoverSlabFilter { get; set; }

        public string ContactPersonFilter { get; set; }

        public string ContactNumberFilter { get; set; }

        public string EmailIDFilter { get; set; }

        public string NationalityFilter { get; set; }

        public string DesignationFilter { get; set; }

        public string VATIDFilter { get; set; }

        public string ParentEntityNameFilter { get; set; }

        public string LegalRepresentativeFilter { get; set; }

        public string ParentEntityCountryCodeFilter { get; set; }

        public string LastReturnFiledFilter { get; set; }

        public string VATReturnFillingFrequencyFilter { get; set; }

        public string TimeZoneFilter { get; set; }

        public int? isPhase1Filter { get; set; }

        public string FaxNoFilter { get; set; }

        public string WebsiteFilter { get; set; }

    }
}