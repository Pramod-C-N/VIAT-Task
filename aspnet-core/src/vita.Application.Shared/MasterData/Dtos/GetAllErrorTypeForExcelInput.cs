using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllErrorTypeForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string CodeFilter { get; set; }

        public string ModuleNameFilter { get; set; }

        public string ErrorGroupIdFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}