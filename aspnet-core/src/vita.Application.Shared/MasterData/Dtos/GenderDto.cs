using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class GenderDto : EntityDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

    }
}