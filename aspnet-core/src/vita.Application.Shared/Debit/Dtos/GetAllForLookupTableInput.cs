using Abp.Application.Services.Dto;

namespace vita.Debit.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}