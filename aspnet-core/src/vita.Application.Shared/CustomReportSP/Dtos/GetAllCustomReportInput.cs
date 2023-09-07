﻿using Abp.Application.Services.Dto;
using System;

namespace vita.CustomReportSP.Dtos
{
    public class GetAllCustomReportInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ReportNameFilter { get; set; }

        public string StoredProcedureNameFilter { get; set; }

    }
}