using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Sales.Dtos
{
    public class GetSalesInvoicePartyForEditOutput
    {
        public CreateOrEditSalesInvoicePartyDto SalesInvoiceParty { get; set; }

    }
}