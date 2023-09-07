﻿using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class SectorDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string GroupName { get; set; }

        public string IndustryGroupCode { get; set; }

        public string IndustryGroupName { get; set; }

        public string IndustryCode { get; set; }

        public string IndustryName { get; set; }

        public string SubIndustryCode { get; set; }

        public string SubIndustryName { get; set; }

        public bool IsActive { get; set; }

    }
}