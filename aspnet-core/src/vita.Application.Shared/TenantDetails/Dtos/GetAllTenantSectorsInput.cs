using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantSectorsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string SubIndustryCodeFilter { get; set; }

        public string SubIndustryNameFilter { get; set; }

    }
}