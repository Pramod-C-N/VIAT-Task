using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class FinancialYearDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string EffectiveFromDate { get; set; }

        public string EffectiveTillEndDate { get; set; }

        public bool IsActive { get; set; }

    }
}