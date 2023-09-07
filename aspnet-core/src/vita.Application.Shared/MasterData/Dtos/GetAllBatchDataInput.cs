using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllBatchDataInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxfromDateFilter { get; set; }
        public DateTime? MinfromDateFilter { get; set; }

        public DateTime? MaxtoDateFilter { get; set; }
        public DateTime? MintoDateFilter { get; set; }

    }
}