using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class ModuleDto : EntityDto<long>
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public int Status { get; set; }

        public int? TenantId { get; set; }

    }
}