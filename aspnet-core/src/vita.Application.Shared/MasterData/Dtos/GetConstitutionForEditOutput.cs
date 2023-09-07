using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetConstitutionForEditOutput
    {
        public CreateOrEditConstitutionDto Constitution { get; set; }

    }
}