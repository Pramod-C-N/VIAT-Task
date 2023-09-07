using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantPurchaseVatCateoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string VATCategoryIDFilter { get; set; }

        public string VATCategoryNameFilter { get; set; }

        public string VATCodeFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}