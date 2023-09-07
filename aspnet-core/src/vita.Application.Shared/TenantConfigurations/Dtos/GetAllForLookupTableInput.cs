using Abp.Application.Services.Dto;

namespace vita.TenantConfigurations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}