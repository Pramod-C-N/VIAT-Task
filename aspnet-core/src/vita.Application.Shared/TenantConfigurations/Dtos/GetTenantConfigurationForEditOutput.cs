using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantConfigurations.Dtos
{
    public class GetTenantConfigurationForEditOutput
    {
        public CreateOrEditTenantConfigurationDto TenantConfiguration { get; set; }

    }
}