using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetModuleForEditOutput
    {
        public CreateOrEditModuleDto Module { get; set; }

    }
}