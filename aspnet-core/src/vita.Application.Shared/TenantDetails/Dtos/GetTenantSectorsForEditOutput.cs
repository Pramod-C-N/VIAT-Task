using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class GetTenantSectorsForEditOutput
    {
        public CreateOrEditTenantSectorsDto TenantSectors { get; set; }

    }
}