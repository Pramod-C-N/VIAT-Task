using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class GetPlaceOfPerformanceForEditOutput
    {
        public CreateOrEditPlaceOfPerformanceDto PlaceOfPerformance { get; set; }

    }
}