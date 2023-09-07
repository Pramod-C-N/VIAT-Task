using Abp.Application.Services.Dto;

namespace vita.PurchaseDebit.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}