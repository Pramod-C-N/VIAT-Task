using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllTitleForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}