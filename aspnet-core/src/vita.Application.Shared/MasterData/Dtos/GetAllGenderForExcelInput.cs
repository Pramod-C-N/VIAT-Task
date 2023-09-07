using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllGenderForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}