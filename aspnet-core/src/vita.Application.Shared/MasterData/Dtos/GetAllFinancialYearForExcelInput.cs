﻿using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllFinancialYearForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string CodeFilter { get; set; }

        public string EffectiveFromDateFilter { get; set; }

        public string EffectiveTillEndDateFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}