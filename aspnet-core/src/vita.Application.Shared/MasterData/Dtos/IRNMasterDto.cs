using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class IRNMasterDto : EntityDto<long>
    {
        public string TransactionType { get; set; }

        public string Status { get; set; }

    }
}