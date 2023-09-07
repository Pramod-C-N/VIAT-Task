using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class ConstitutionDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}