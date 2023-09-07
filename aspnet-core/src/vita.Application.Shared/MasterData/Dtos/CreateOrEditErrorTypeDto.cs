using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditErrorTypeDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string ModuleName { get; set; }

        public string ErrorGroupId { get; set; }

        public bool IsActive { get; set; }

    }
}