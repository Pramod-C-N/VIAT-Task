using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class BatchDataDto : EntityDto<long>
    {
        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }

    }
}