using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditIRNMasterDto : EntityDto<long?>
    {

        public string TransactionType { get; set; }

        public string Status { get; set; }

    }
}