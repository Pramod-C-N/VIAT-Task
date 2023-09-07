using System;
using Abp.Application.Services.Dto;

namespace vita.CustomReportSP.Dtos
{
    public class CustomReportDto : EntityDto
    {
        public string ReportName { get; set; }

        public string StoredProcedureName { get; set; }

    }
}