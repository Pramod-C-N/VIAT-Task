using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantBusinessPurchaseInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BusinessTypeIDFilter { get; set; }

        public string BusinessPurchaseFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}