using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantBusinessSuppliesDto : EntityDto<int?>
    {

        public string BusinessTypeID { get; set; }

        public string BusinessSupplies { get; set; }

        public bool IsActive { get; set; }

    }
}