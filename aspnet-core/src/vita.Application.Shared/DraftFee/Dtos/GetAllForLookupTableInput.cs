using Abp.Application.Services.Dto;

namespace vita.DraftFee.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}