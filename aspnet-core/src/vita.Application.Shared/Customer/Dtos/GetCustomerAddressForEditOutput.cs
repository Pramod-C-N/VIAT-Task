using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerAddressForEditOutput
    {
        public CreateOrEditCustomerAddressDto CustomerAddress { get; set; }

    }
}