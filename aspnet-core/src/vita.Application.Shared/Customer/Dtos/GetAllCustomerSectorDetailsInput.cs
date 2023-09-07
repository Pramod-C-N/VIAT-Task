﻿using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerSectorDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

        public string SubIndustryCodeFilter { get; set; }

        public string SubIndustryNameFilter { get; set; }

    }
}