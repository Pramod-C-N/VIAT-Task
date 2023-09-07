using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerTaxDetailsForEditOutput
    {
        public CreateOrEditCustomerTaxDetailsDto CustomerTaxDetails { get; set; }

    }
}