using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.CustomReportSP.Dtos
{
    public class CreateOrEditCustomReportDto : EntityDto<int?>
    {

        public string ReportName { get; set; }

        public string StoredProcedureName { get; set; }

    }
}