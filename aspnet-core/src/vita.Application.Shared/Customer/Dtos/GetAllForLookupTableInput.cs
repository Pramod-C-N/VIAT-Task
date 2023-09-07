using Abp.Application.Services.Dto;

namespace vita.Customer.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}