using Abp.Application.Services.Dto;

namespace vita.Vendor.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}