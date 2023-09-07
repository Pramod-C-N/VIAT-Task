using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditModuleDto : EntityDto<long?>
    {

        public int ModuleId { get; set; }

        [Required]
        public string ModuleName { get; set; }

        public int Status { get; set; }

        public int? TenantId { get; set; }

    }
}