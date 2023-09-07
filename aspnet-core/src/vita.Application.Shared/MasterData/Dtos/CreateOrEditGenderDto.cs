using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditGenderDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public bool IsActive { get; set; }

    }
}