using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantPurchaseVatCateoryDto : EntityDto<int?>
    {

        public string VATCategoryID { get; set; }

        public string VATCategoryName { get; set; }

        public string VATCode { get; set; }

        public bool IsActive { get; set; }

    }
}