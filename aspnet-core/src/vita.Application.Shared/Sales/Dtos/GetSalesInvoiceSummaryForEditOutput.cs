﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Sales.Dtos
{
    public class GetSalesInvoiceSummaryForEditOutput
    {
        public CreateOrEditSalesInvoiceSummaryDto SalesInvoiceSummary { get; set; }

    }
}