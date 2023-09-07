using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class GetTenantPurchaseVatCateoryForEditOutput
    {
        public CreateOrEditTenantPurchaseVatCateoryDto TenantPurchaseVatCateory { get; set; }

    }
}