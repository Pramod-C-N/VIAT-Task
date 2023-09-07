using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantSupplyVATCategoryDto : EntityDto
    {
        public string VATCategoryID { get; set; }

        public string VATCategoryName { get; set; }

        public string VATCode { get; set; }

        public bool IsActive { get; set; }

    }
}