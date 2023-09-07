using Abp.Application.Services.Dto;

namespace vita.Sales.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}