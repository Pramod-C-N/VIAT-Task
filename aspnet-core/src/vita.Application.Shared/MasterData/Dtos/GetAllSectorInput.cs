using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllSectorInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string CodeFilter { get; set; }

        public string GroupNameFilter { get; set; }

        public string IndustryGroupCodeFilter { get; set; }

        public string IndustryGroupNameFilter { get; set; }

        public string IndustryCodeFilter { get; set; }

        public string IndustryNameFilter { get; set; }

        public string SubIndustryCodeFilter { get; set; }

        public string SubIndustryNameFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}