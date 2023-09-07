using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllModuleInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxModuleIdFilter { get; set; }
        public int? MinModuleIdFilter { get; set; }

        public string ModuleNameFilter { get; set; }

        public int? MaxStatusFilter { get; set; }
        public int? MinStatusFilter { get; set; }

        public int? MaxTenantIdFilter { get; set; }
        public int? MinTenantIdFilter { get; set; }

    }
}