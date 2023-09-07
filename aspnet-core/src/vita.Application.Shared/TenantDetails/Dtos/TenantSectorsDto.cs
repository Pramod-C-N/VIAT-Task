using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantSectorsDto : EntityDto
    {
        public string SubIndustryCode { get; set; }

        public string SubIndustryName { get; set; }

    }
}