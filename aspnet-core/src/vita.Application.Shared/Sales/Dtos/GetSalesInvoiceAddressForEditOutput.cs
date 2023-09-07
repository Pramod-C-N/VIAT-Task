using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Sales.Dtos
{
    public class GetSalesInvoiceAddressForEditOutput
    {
        public CreateOrEditSalesInvoiceAddressDto SalesInvoiceAddress { get; set; }

    }
}