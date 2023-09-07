using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class GetTenantBusinessSuppliesForEditOutput
    {
        public CreateOrEditTenantBusinessSuppliesDto TenantBusinessSupplies { get; set; }

    }
}