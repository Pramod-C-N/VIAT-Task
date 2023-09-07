using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class TitleDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}