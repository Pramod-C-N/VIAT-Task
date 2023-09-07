using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.CustomReportSP.Dtos
{
    public class GetCustomReportForEditOutput
    {
        public CreateOrEditCustomReportDto CustomReport { get; set; }

    }
}