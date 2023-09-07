using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}