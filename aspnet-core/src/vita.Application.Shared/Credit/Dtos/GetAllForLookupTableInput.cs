using Abp.Application.Services.Dto;

namespace vita.Credit.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}