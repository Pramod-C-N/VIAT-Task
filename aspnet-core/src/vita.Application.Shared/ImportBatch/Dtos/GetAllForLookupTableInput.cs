using Abp.Application.Services.Dto;

namespace vita.ImportBatch.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}