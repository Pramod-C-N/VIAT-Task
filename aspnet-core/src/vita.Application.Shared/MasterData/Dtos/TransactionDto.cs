using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace vita.MasterData.Dtos
{
    public class TransactionDto : EntityDto
    {
        public long IRNNo { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public Guid UniqueIdentifier { get; set; }
    }
}
