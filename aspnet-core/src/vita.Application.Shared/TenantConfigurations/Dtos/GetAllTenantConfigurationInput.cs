using Abp.Application.Services.Dto;
using System;

namespace vita.TenantConfigurations.Dtos
{
    public class GetAllTenantConfigurationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}