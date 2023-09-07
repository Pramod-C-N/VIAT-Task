using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantBusinessPurchaseDto : EntityDto
    {
        public string BusinessTypeID { get; set; }

        public string BusinessPurchase { get; set; }

        public bool IsActive { get; set; }

    }
}