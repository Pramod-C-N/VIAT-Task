using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantBusinessSuppliesDto : EntityDto
    {
        public string BusinessTypeID { get; set; }

        public string BusinessSupplies { get; set; }

        public bool IsActive { get; set; }

    }
}