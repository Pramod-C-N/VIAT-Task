using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantBusinessPurchaseDto : EntityDto<int?>
    {

        public string BusinessTypeID { get; set; }

        public string BusinessPurchase { get; set; }

        public bool IsActive { get; set; }

    }
}