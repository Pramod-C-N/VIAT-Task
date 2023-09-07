using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllIRNMastersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TransactionTypeFilter { get; set; }

        public string StatusFilter { get; set; }

    }
}