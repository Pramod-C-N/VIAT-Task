using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetBusinessProcessForEditOutput
    {
        public CreateOrEditBusinessProcessDto BusinessProcess { get; set; }

    }
}