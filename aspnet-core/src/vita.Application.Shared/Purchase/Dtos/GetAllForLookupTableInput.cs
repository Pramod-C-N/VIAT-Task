using Abp.Application.Services.Dto;

namespace vita.Purchase.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}