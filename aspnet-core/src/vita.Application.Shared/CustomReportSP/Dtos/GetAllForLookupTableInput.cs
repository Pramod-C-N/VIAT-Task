using Abp.Application.Services.Dto;

namespace vita.CustomReportSP.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}