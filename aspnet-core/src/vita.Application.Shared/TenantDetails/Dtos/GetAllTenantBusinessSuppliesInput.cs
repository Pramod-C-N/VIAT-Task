using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantBusinessSuppliesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BusinessTypeIDFilter { get; set; }

        public string BusinessSuppliesFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}