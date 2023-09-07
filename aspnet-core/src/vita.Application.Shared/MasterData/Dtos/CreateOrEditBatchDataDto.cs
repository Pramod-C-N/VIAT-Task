using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditBatchDataDto : EntityDto<long?>
    {

        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }

    }
}